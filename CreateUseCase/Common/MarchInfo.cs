using System;
using System.Collections.Generic;

namespace CreateUseCase
{
    public static class MatchInfo
    {
        public static string GetParams(List<DataDTO> data)
        {
            string _data = string.Empty;

            foreach (var item in data)
            {
                _data = _data + item.Param + ",";
            }

            return _data.Remove(_data.Length - 1);
        }

        public static string GetVariables(List<DataDTO> data)
        {
            string _data = string.Empty;

            foreach (var item in data)
            {
                _data = _data + "\n\t" + item.Variable;
            }

            return _data.ToString();
        }

        public static string GetConstructor(List<DataDTO> data)
        {
            string _data = string.Empty;

            foreach (var item in data)
            {
                _data = _data + "\n" + item.AssignConstructor;
            }

            return _data.ToString();
        }

        public static string GetFunctions(List<DataDTO> data)
        {
            string _data = string.Empty;

            foreach (var item in data)
            {

                _data = _data + "\n\t" + item.Function;
            }

            return _data.ToString();
        }

        internal static string GetCreateSchemaJoi(List<DataDTO> data)
        {
            if (data[0].type == "id")
            {
                data.Remove(data[0]);
            }

            string pattern = "";

            foreach (var item in data)
            {
                pattern += item.SchemaJoi;    
            }

            return pattern;
        }

        internal static string GetEditSchemaJoi(List<DataDTO> data)
        {
            string pattern = "";

            foreach (var item in data)
            {
                pattern += item.SchemaJoi;    
            }

            return pattern;
        }

        internal static string GetFindByIdSchemaJoi(List<DataDTO> data)
        {
            return "id: Joi.number().min(0).required(),";
        }

        internal static string GetFindSchemaJoi(List<DataDTO> data)
        {
            string pattern = "";

            foreach (var item in data)
            {
                pattern += item.SchemaJoi;    
            }

            return pattern;
        }

        internal static string GetPropsEntityTs(List<DataDTO> data)
        {
            string pattern = "";
            if (data[0].type == "id")
            {
                foreach (var item in data)
                {
                    pattern += string.Format("\t{0}\n\t{1}", item.PropertyEntity, item.ValueEntity);
                }
            }
            else
            {
                pattern = ("\t@PrimaryGeneratedColumn()\n\tpublic Id!: number");

                foreach (var item in data)
                {
                    pattern += string.Format("\t{0}\n\t{1}", item.PropertyEntity, item.ValueEntity);
                }
            }

            return pattern;
        }

        internal static string GetDeleteSchemaJoi(List<DataDTO> data)
        {
            return "id: Joi.number().min(0).required(),";
        }
    }
}