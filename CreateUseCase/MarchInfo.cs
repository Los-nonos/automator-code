using System;
using System.Collections.Generic;
using System.Text;

namespace CreateUseCase
{
    public static class MatchInfo
    {
        public static string GetParams(List<DataDTO> data)
        {
            string _data = string.Empty;

            foreach (var item in data)
            {
                _data = _data + item.GetParam() + ",";
            }

            return _data.Remove(_data.Length - 1);
        }

        public static string GetVariables(List<DataDTO> data)
        {
            string _data = string.Empty;

            foreach (var item in data)
            {
                _data = _data + item.GetVariable();
            }

            return _data.ToString();
        }

        public static string GetConstructor(List<DataDTO> data)
        {

            string _data = string.Empty;

            foreach (var item in data)
            {
                _data = _data + item.GetConstructor();
            }

            return _data.ToString();
        }

        public static string GetFunctions(List<DataDTO> data)
        {
            string _data = string.Empty;

            foreach (var item in data)
            {

                _data = _data + item.GetFunction();
            }

            return _data.ToString();
        }
    }
}