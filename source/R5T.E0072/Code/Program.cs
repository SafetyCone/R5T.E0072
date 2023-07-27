using System;
using System.Threading.Tasks;


namespace R5T.E0072
{
    class Program
    {
        static async Task Main()
        {
            await Experiments.Instance.GetAndSerializeStrangeObject();
            //Experiments.Instance.DeserializeStrangeObject();
        }
    }
}