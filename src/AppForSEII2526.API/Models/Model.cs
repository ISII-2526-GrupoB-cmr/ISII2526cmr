namespace AppForSEII2526.API.Models { 

    public class Model
    {

         public int ID { get; set; }
         public string Name { get; set; }


        public IList<Car> Cars { get; set; }
        public Model(){}
        public Model(string name)
        {
                       Name = name;
        }
    }

}