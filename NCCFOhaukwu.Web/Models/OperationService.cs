using System;
using System.Linq;
using DAL;

namespace NCCFOhaukwu.Web.Models
{
    public class OperationService
    {
        public static Operations Op(string[] seleectedOperations)
        {
            return seleectedOperations.Aggregate<string, Operations>(0,
                (current, seleectedOperation) => current + Convert.ToInt32(seleectedOperation));
        }

        public static Operations Op(string operation)
        {
            Operations op = 0;
            op += Convert.ToInt32(operation);
            return op;
        }
    }
}