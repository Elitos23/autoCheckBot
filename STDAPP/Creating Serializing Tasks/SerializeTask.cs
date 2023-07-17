using System;
using System.Collections.Generic;

namespace STDAPP
{
    [Serializable]
    public class SerializeTask
    {

        public List<TaskData> taskDatas = new List<TaskData>();

    }
    public class TaskData
    {
        public bool IsDeleted = false;

        public int Shop { get; set; }

        public string accout { get; set; }

        public string proxy { get; set; }

        public string size { get; set; }

        public string time { get; set; }

        public string profile { get; set; }

        public DataSourceStructures.DataGive DataGive;

        public DataSourceStructures.DataEdit DataEdit;


        //public Task Tsk =  new Task(() => Console.WriteLine("inisializing task"));

    }

   
}
