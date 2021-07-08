using Mafia2Tool;
using ResourceTypes.Actors;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.IO
{
    class FileActor : FileBase
    {
        public FileActor(FileInfo info) : base(info)
        {
        }

        public List<string> GetDefinitionList()
        {
            try
            {
                Actor actors = new Actor(file.FullName);

                List<string> definitions = new List<string>();

                for (int i = 0; i < actors.Definitions.Count; i++)
                {
                    definitions.Add(actors.Definitions[i].Name);
                }

                return definitions;
            }
            catch
            {
                string Message = string.Format("ERROR: Failed to read actor file: {0}", GetName());
                Console.WriteLine(Message);

                return new List<string>();
            }
        }

        public override string GetExtensionUpper()
        {
            return "ACT";
        }

        public override bool Open()
        {
            ActorEditor editor = new ActorEditor(file);
            return true;
        }
    }
}
