using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace rMind.Device
{
    using Core;
    using System.IO;

    public class NRequest : Device
    {
        public NRequest(IMindCore board) :base(board) { }

        public override void Execute()
        {
            var request = WebRequest.Create("http://samples.openweathermap.org/data/2.5/weather?zip=94040,us&appid=b6907d289e10d714a6e88b30761fae22");
            var response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            response.Close();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
