using Automix_ini.model.config;

namespace Automix_ini.model.format
{
    public interface IData
    {
        string IniDataToString(data iniData);

        ini_config Configuration { get; set; }
    }

}