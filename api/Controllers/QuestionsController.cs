using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("api/Questions")]
    public class QuestionsController : ControllerBase
    {
        readonly QuizContext context;
        
        public QuestionsController(QuizContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Models.Question> Get()
        {
            return context.Questions;

        }

        [HttpGet("{quizId}")]
        public IEnumerable<Models.Question> Get([FromRoute] int quizId)
        {
            return context.Questions.Where(q => q.QuizId == quizId);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Question question)
        {
            var quiz = context.Quiz.SingleOrDefault(q => q.ID == question.QuizId);

            if (quiz == null)
                return NotFound();

            context.Questions.Add(question);
            await context.SaveChangesAsync();

            return Ok(question);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Models.Question questionData)
        {
            //var question = await context.Questions.SingleOrDefaultAsync(q => q.ID == id);
            if (id != questionData.ID)
                return BadRequest();

            context.Entry(questionData).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return Ok(questionData);
        }
    }
}