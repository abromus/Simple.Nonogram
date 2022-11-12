using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Simple.Nonogram.Extensions;
using Simple.Nonogram.Infrastructure.Delegates;
using SimpleJSON;

namespace Simple.Nonogram.Configuration
{
    public class GameConfiguration
    {
        public string Product { get; private set; }
        public string Version { get; private set; }
        public ulong Code { get; private set; }

        public Data Data { get; private set; }
        public Collections Collections { get; private set; }

        private GameConfiguration(JSONNode mainNode)
        {
            Product = mainNode["product"].Value;
            Version = mainNode["version"].Value;
            Code = mainNode["code"].AsULong;

            Data = new Data(mainNode["data"]);

            Collections = new Collections(mainNode["collections"]);
        }

        public static GameConfiguration PreInitialize(JSONNode mainConfig)
        {
            return new GameConfiguration(mainConfig);
        }

        public static async Task<GameConfiguration> PreInitialize(string mainConfigString)
        {
            JSONNode mainConfig = null;

            List<UniTask> jsonParseList = new List<UniTask>();
            jsonParseList.Add(ParseJSON(mainConfigString, node => mainConfig = node));

            await UniTask.WhenAll(jsonParseList);

            return new GameConfiguration(mainConfig);
        }

        private static async UniTask ParseJSON(string json, Block<JSONNode> nodeBlock)
        {
            JSONNode node = null;

            await Task.Run(() => node = JSONNode.Parse(json));

            nodeBlock.SafeInvoke(node);
        }
    }
}
