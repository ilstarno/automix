using System;

namespace Automix_ini.model
{
   
    public class non_sense_data : data
    {
        public non_sense_data()
            : base(new section_collection(StringComparer.OrdinalIgnoreCase))
        {
            Global = new key_collection(StringComparer.OrdinalIgnoreCase);
        }

        public non_sense_data(section_collection sdc)
            : base(new section_collection(sdc, StringComparer.OrdinalIgnoreCase))
        {
            Global = new key_collection(StringComparer.OrdinalIgnoreCase);
        }

        public non_sense_data(data ori)
            : this(new section_collection(ori.Sections, StringComparer.OrdinalIgnoreCase))
        {
            Global = (key_collection)ori.Global.Clone();
            Configuration = ori.Configuration.Clone();
        }
    }

}