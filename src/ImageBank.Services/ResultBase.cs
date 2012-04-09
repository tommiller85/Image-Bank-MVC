using System.Collections.Generic;

namespace ImageBank.Services
{
    public abstract class ResultBase
    {
        public List<string> Errors { get; set; }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }
    }
}