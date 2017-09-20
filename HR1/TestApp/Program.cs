using AppCore.Modules.Foundation.DomainModel;
using HR1.DomainModel;
using HR1.DomainModel.Context;
using AppCore.Modules.HR.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IHR1Context db = new HR1Context(null, null);
            //AccountingEntity entity = new AccountingEntity();
            //entity.Party = new Party() { FirstName = "Test", Test = 5 };
            //entity.Employees.Add(new Employee()
            //{
            //    Party = new Party() { FirstName = "Emp1", Test = 7 },
            //    EmployeeNumber = "123"
            //});

            db.AccountingEntities.ToList();
            //db.AccountingEntities.Add(entity);
            //db.SaveChanges();

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
