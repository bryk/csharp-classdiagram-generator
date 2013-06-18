using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IndexerLogic; 
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace IndexerProgram
{
    public class MainIndexer
    {
        public static int Main()
        {

            DBModule.DBConnection connection = new DBModule.DBConnection();

            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = "unity.config";
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            UnityConfigurationSection section = (UnityConfigurationSection)config.GetSection("unity");
   
            IUnityContainer myContainer = new UnityContainer();
            section.Configure(myContainer, "DatabaseContainer");
            DBModule.IDataBaseIndexer database = myContainer.Resolve<DBModule.IDataBaseIndexer>("DataBase");
            DBModule.IDataBaseGetDocument fifo = myContainer.Resolve<DBModule.IDataBaseGetDocument>("DataBaseFifo");
            /*
            DBModule.IDataBaseIndexer database = new DBModule.EntityHandler();
            DBModule.IDataBaseGetDocument fifo = new DBModule.DocumentFromFifo();
            */
            Indexer indexer = new Indexer(database, fifo);
            indexer.ReadProducersList();
            Thread indexerThread = new Thread(new ThreadStart(indexer.Run));
            indexerThread.Start();
            indexerThread.Join();
            return 0;
        }

    }
}