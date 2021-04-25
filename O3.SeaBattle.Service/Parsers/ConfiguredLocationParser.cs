namespace O3.SeaBattle.Service.Parsers
{
    public class ConfiguredLocationParser : LocationParser
    {
        public ConfiguredLocationParser() :
            base(GameConfig.RowOrigin, GameConfig.ColOrigin, GameConfig.MaxSize)
        {
        }
    }
}
