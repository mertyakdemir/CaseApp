using MediatR;
using Membership.API.CQRS.Commands.Request;
using Membership.API.CQRS.Commands.Response;
using Membership.API.CQRS.Queries.Request;
using Membership.API.CQRS.Queries.Response;
using Membership.API.Models;
using Membership.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Membership.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GenerateRef _generateRef;
        private readonly ApplyRefCode _applyRefCode;
        readonly IMediator _mediator;

        public UserController(GenerateRef generateRef, ApplyRefCode applyRefCode, IMediator mediator)
        {
            _generateRef = generateRef;
            _applyRefCode = applyRefCode;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login(GetUserQueryRequest loginModel)
        {
            GetUserQueryResponse user = await _mediator.Send(loginModel);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> MyReferenceDetails(GetByIdUserQueryRequest userModel)
        {
            GetByIdUserQueryResponse userdetails = await _mediator.Send(userModel);
            return Ok(userdetails);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(CreateUserCommandRequest registerModel, string signupRefCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please fill in the required fields.");
            }

            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://rmvawcvo:xBrOAZlvrZX_MAvUYvMyKEqIz6pWVOPp@snake.rmq2.cloudamqp.com/rmvawcvo")
            };

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("user-exchange", ExchangeType.Direct, true, false, null);

            channel.QueueDeclare(queue: "Email", durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.QueueBind("Email", "user-exchange", "SendEmail");

            var refCode = "";
            var uri = $"{Request.Scheme}://{Request.Host}" + Url.Action("SignUp", "User", new { signupRefCode = string.Empty });

            var generateRefCode = _generateRef.GenerateRefCode();

            var validateRefCode = _generateRef.ValidateRefCode(generateRefCode).Result;

            if (signupRefCode == null)
            {
                refCode = _applyRefCode.AssignRefCode(registerModel.RefCode);
            }
            else
            {
                refCode = _applyRefCode.AssignRefCode(signupRefCode);
            }

            if (refCode == string.Empty && (signupRefCode != null || registerModel.RefCode != null))
            {
                return BadRequest("Reference code not exists " + ": " + (signupRefCode == null ? $"{registerModel.RefCode}" : $"{signupRefCode}"));
            }

            if (validateRefCode == true && ModelState.IsValid)
            {
                registerModel.GenerateRef = generateRefCode;
                registerModel.RefCode = refCode;
                registerModel.RefUri = uri + "/" + generateRefCode;
            }

            CreateUserCommandResponse response = await _mediator.Send(registerModel);

            var messageBody = JsonSerializer.Serialize(registerModel);
            var convertMessage = Encoding.UTF8.GetBytes(messageBody);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish("user-exchange", routingKey: "SendEmail", basicProperties: properties, body: convertMessage);

            return Ok(response);
        }
    }
}
