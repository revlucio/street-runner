using System;

namespace StreetRunner.Core.Mapping
{
    public class StravaRunParser
    {
        private readonly StravaJsonRun _run;
        private readonly bool _valid;

        public StravaRunParser(string json, string id)
        {
            try
            {
                _run = new StravaJsonRun(json, id);
                _valid = true;
            }
            catch
            {
                _valid = false;
            }
        }

        public IRun Value => _valid 
            ? _run 
            : throw new ArgumentException("JSON could not be parsed into a StravaJsonRun");

        public bool IsValid() => _valid;
    }
}