using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SlackBotMessages;
using SlackBotMessages.Models;
using WageClerk.Data;
using WageClerk.Data.Models;
using WagesClerk.Data;
using WagesClerk.Models;

namespace WagesClerk.Controllers.Slack
{
    [Route("api/shift")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IEmployeeShiftDataRepository _employeeShiftData;
        private readonly IEmployeeDataRepository _employeeRepo;
        private readonly IShiftDataRepository _shiftData;
        private readonly IConfiguration _configuration;

        public ShiftController(IEmployeeShiftDataRepository employeeShiftData, IEmployeeDataRepository employeeData, IShiftDataRepository shiftData, IConfiguration configuration)
        {
            _employeeShiftData = employeeShiftData;
            _employeeRepo = employeeData;
            _shiftData = shiftData;
            _configuration = configuration;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("clockIn")]
        public string PostClockInTime([FromForm] SlashCommand slashCommand)
        {
            //plain text for now
            var response =
                "Looks like the command you entered is not quite right :worried:. If you want to clock in make sure you use the command /clockin.";
            //check if incoming command is correct
            if (!String.IsNullOrEmpty(slashCommand.text))
            {
                SendMessage(slashCommand.user_name, response);
                return response;
            }

            var userName = slashCommand.user_name;
            var employee = _employeeRepo.GetByUserName(userName);

            if(employee == null)
            {
                employee = _employeeRepo.Add(new WageClerk.Data.Models.Employee
                {
                    UserName = userName
                });
                response = _shiftData.SetClockInTime(employee);
                SendMessage(slashCommand.user_name, response);
                return response;
            }

            //Check if already clocked in 
            if (_shiftData.GetClockedInShift(employee) == null)
            {
                response = _shiftData.SetClockInTime(employee);
                SendMessage(slashCommand.user_name, response);
                return response;
            }

            response = "You are already clocked in :timer_clock:! Please clock out first.";
            SendMessage(slashCommand.user_name, response);
            return response;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("clockOut")]
        public string PostClockOutTime([FromForm] SlashCommand slashCommand)
        {
            try
            {
                var response = "";
                var userName = slashCommand.user_name;
                var employee = _employeeRepo.GetByUserName(userName);
                //check if incoming command is correct and the employee exists using username
                if (!String.IsNullOrEmpty(slashCommand.text) || employee == null)
                {
                    throw new Exception();
                }

                var shiftToClockOut = _shiftData.GetClockedInShift(employee);
                if (shiftToClockOut != null)
                {
                    response = _shiftData.SetClockOutTime(shiftToClockOut, employee);
                    SendMessage(slashCommand.user_name, response);
                    return response;
                }

                response = "You have not clocked in yet :timer_clock:! Please clock in before you clock out.";
                SendMessage(slashCommand.user_name, response);
                return response;
            }

            catch (Exception e)
            {
                var response =
                    "Looks like the command you entered is not quite right :worried:. If you want to clock out make sure you use the command /clockout.";
                SendMessage(slashCommand.user_name, response);
                return response;
            }
           
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("morale")]
        public string GetAverageShiftRating([FromForm] SlashCommand slashCommand)
        {
            try
            {
                var userName = slashCommand.user_name;
                var employee = _employeeRepo.GetByUserName(userName);
                //check if incoming command is correct and the employee exists using username
                if (!String.IsNullOrEmpty(slashCommand.text) || employee == null)
                {
                    throw new Exception();
                }

                return _shiftData.GetEmployeeMorale(employee);

            }
            catch (Exception e)
            {
                return "Oops an error occurred :worried:. Please try again.";
            }
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        [Route("rating")]
        public string PostShiftRating([FromForm] SlashCommand slashCommand)
        {
            try
            {
                //rating can only be between 1-4
                var rating = Int32.Parse(slashCommand.text);
                var ratingList = new List<int> {1, 2, 3, 4};

                if (String.IsNullOrEmpty(slashCommand.text) || !ratingList.Contains(rating))
                {
                    throw new Exception();
                }
                
                var userName = slashCommand.user_name;
                var employee = _employeeRepo.GetByUserName(userName);
                return _shiftData.SetShiftRating(employee, (Rating)rating);
            }

            catch (Exception e)
            {
                return "The command doesn't seem right :worried:. Use /ratemyshift <rating 1-4> to rate your latest shift.";
            }
        }

        private void SendMessage(string message, string userName)
        {
            var webHookUrl = _configuration["webhookUrl"];
            var client = new SbmClient(webHookUrl);

            var messageRequest = new Message($"Dear {userName}, {message}")
                .SetUserWithEmoji("Website", Emoji.Loudspeaker);

            messageRequest.AddAttachment(new Attachment()
                .AddField("Team", "Wages Clerk", true)
                .AddField("Company", "Ready tech", true)
                .AddField("Email", "wagesclerk@mail.com", true)
                .SetColor("#f96332")
            );
            client.Send(messageRequest);
        }
    }
}
