using System.Collections.Generic;

namespace ELearningV1._3._1.Models
{
    public class ErrorContext {
        public IList<string> Errors {get; set;} = new List<string>();
    }

}