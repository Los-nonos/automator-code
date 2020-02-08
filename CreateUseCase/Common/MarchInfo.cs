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
                _data = _data + "\n \t" + item.Variable;
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

                _data = _data + "\n \t" + item.Function;
            }

            return _data.ToString();
        }

        // TODO: Implement
        internal static string GetCreateSchemaJoi(List<DataDTO> data)
        {
            throw new NotImplementedException();
        }

        internal static string GetEditSchemaJoi(List<DataDTO> data)
        {
            throw new NotImplementedException();
        }

        internal static string GetFindByIdSchemaJoi(List<DataDTO> data)
        {
            throw new NotImplementedException();
        }

        internal static string GetFindSchemaJoi(List<DataDTO> data)
        {
            throw new NotImplementedException();
        }

        internal static string GetPropsEntityTs(List<DataDTO> data)
        {
            throw new NotImplementedException();
        }
    }
}