using DiplomApi.Data.Models;
using DiplomApi.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Serialization;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using Object = DiplomApi.Data.Models.Object;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DiplomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        // GET: api/<ChatsController>
        [HttpGet("getChats")]
        public async Task<IActionResult> Get()
        {
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObject) && userIdObject is string userId)
            {
                var user = await Program.context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userIdObject);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    UserChatsViewModel chatsViewModel = new UserChatsViewModel();
                    chatsViewModel.PrivateChats = new List<UserChatViewModel>();
                    chatsViewModel.ObjectChats = new List<Object>();
                    chatsViewModel.MainChat = new Chat();

                    var userChats = Program.context.Chats
                        .Where(chat => chat.Users.Any(user => user.Id.ToString() == userId))
                        .ToList();

                    foreach (var chat in userChats)
                    {

                        switch (chat.ChatType)
                        {
                            case ChatTypes.main:
                                {
                                    chatsViewModel.MainChat = chat;
                                }
                                break;
                            case ChatTypes.privat:
                                {

                                    UserChatViewModel viewModel = new UserChatViewModel(user.Id);
                                    viewModel.PrivateChat = chat;

                                    chatsViewModel.PrivateChats.Add(viewModel);
                                }
                                break;
                            case ChatTypes.group:
                                {
                                    Object objectItem = new Object();
                                    objectItem = Program.context.Objects.First(o => o.Chat.Id == chat.Id);
                                    objectItem.Chat = chat;
                                    objectItem.Users = chat.Users.ToList();

                                    chatsViewModel.ObjectChats.Add(objectItem);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    var jsonString = JsonConvert.SerializeObject(chatsViewModel);

                    return Ok(jsonString);
                }
            }

            return BadRequest();
        }

        // GET api/<ChatsController>/5
        [HttpGet("getMessages")]
        public async Task<List<Message>> Get(int id)
        {
            return await Program.context.Message.Where(x=>x.Chat.Id==id).ToListAsync();
        }

        [HttpPost("createChat")]
        public async Task<IActionResult> CreatePrivateChat(string secondUserId)
        {
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObject) && userIdObject is string userId)
            {
                var chat = new Chat();
                chat.ChatType = ChatTypes.privat;
                var chatUsers = Program.context.Users.Where(u => u.Id.ToString() == secondUserId || u.Id.ToString() == userId);
                chat.Users = chatUsers;

                await Program.context.Chats.AddAsync(chat);
                await Program.context.SaveChangesAsync();

                return Ok(chat.Id);
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST api/<ChatsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ChatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
