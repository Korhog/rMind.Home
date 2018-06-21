using System;
using System.Xml.Linq;

using Windows.Storage;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Storage
{
    public class rMindStorage
    {
        string m_tmp_file_name;

        static rMindStorage m_instance;
        static object sync = new Object();

        rMindStorage()
        {
            m_tmp_file_name = "tmp.xml";
        }

        public static rMindStorage GetInstance()
        {
            if(m_instance == null)
            {
                lock(sync)
                {
                    if (m_instance == null)
                    {
                        m_instance = new rMindStorage();
                    }
                }
            }

            return m_instance;
        }

        /// <summary> Save temp project to local storage </summary>
        /// <param name="xml"></param>
        public async Task SaveTmpData(XDocument xml)
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;
            var tmpFile = await local.CreateFileAsync(m_tmp_file_name, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(tmpFile, xml.ToString());
        }

        /// <summary> Load temp project from local storage </summary>
        public async Task<XDocument> LoadTmpData()
        {
            StorageFile tmpFile;
            StorageFolder local = ApplicationData.Current.LocalFolder;
            try
            {
                tmpFile = await local.GetFileAsync(m_tmp_file_name);
            }
            catch
            {
                return new XDocument();
            }
            
            string text = await FileIO.ReadTextAsync(tmpFile);

            if (string.IsNullOrEmpty(text))
                return new XDocument();

            XDocument doc = XDocument.Parse(text);
            return doc;
        }
    }
}
