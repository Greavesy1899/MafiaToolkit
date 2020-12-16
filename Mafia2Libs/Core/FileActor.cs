using Mafia2Tool;
using ResourceTypes.Actors;
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
            Actor actors = new Actor(file.FullName);

            List<string> definitions = new List<string>();

            for (int i = 0; i < actors.Definitions.Count; i++)
            {
                definitions.Add(actors.Definitions[i].Name);
            }

            return definitions;
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
