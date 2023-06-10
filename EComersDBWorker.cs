using EComersObjectLib;
using EComersObjectLib.Objects;
using EComersObjectLib.SapObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8602

namespace EComersDBworkerLib
{
    public class EComersDBWorker
    {
        EComersContext eComersContext;

        public EComersDBWorker(EComersContext _eComersContext)
        {
            eComersContext = _eComersContext;
        }

        public string CreateEmploye(Employe employe, EmployeHistory employeHistory )
        {
            DateTime curentDate = DateTime.UtcNow;
            try
            {
                if (employe == null) throw new ArgumentNullException(nameof(employe));
                if (employeHistory == null) throw new ArgumentNullException(nameof(employeHistory));

                employe.CreatedDate = curentDate;
                eComersContext.Employe.Add(employe);
                eComersContext.SaveChanges();

                employeHistory.EmployeId = employe.EmployeId;
                employeHistory.StartDate = curentDate;
                employeHistory.EndDate = DateTime.MaxValue;
                employeHistory.StatusId = 1;
                employeHistory.NumHistory = 1;

                eComersContext.EmployeHistory.Add(employeHistory);
                eComersContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "0";
        }

        public async Task<string> UpdateEmploye(Employe employe, EmployeHistory employeHistory)
        {
            DateTime curentDate = DateTime.UtcNow;
            try
            {
                if (employe == null) throw new ArgumentNullException(nameof(employe));
                if (employeHistory == null) throw new ArgumentNullException(nameof(employeHistory));

                employe.UpdatedDate = curentDate;
                employeHistory.EndDate = curentDate;
                
                eComersContext.Entry(employe).State = EntityState.Modified;
                eComersContext.Entry(employeHistory).State = EntityState.Modified;
                eComersContext.SaveChanges();

                EmployeHistory? newEmployeHistory = new EmployeHistory().GetNewEmoloyeHistrory(employeHistory);

                if (newEmployeHistory == null) throw new ArgumentException(nameof(newEmployeHistory));

                eComersContext.EmployeHistory.Add(newEmployeHistory);
                int rowCount = await eComersContext.SaveChangesAsync();
                return rowCount.ToString();
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DelleteEmploye (int id)
        {
            
            try
            {
                DateTime curDate = DateTime.Now;
                Employe? employe = await eComersContext.Employe.FirstOrDefaultAsync(p=>p.EmployeId == id);
                EmployeHistory? employeHistory = await eComersContext.EmployeHistory.FirstOrDefaultAsync(p=> p.EmployeId == employe.EmployeId && p.StartDate < curDate && p.EndDate > curDate);

                if (employeHistory == null || employe == null) throw new ArgumentException(nameof(employe));

                employeHistory.StatusId = 2;
                return await UpdateEmploye(employe, employeHistory);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<Employe>> GetEmployeListAsync()
        {
            if (eComersContext == null)
            {
                return new List<Employe>();
            }

            var employeList = await eComersContext.Employe
                .Include(p => p.EmployeHistory).ThenInclude(p => p.EmployeStatus)
                .Include(p => p.EmployeHistory).ThenInclude(p => p.FunctionsType)
                .Include(p => p.EmployeHistory).ThenInclude(p => p.Gender)
                .ToListAsync();
            
            return employeList;
        }

        public async Task<Employe?> GetEmployeByIdAsync(int id)
        {
            if (eComersContext == null)
            {
                return new Employe();
            }

            var employe = await eComersContext.Employe
                .Include(p => p.EmployeHistory).ThenInclude(p => p.EmployeStatus)
                .Include(p => p.EmployeHistory).ThenInclude(p => p.FunctionsType)
                .Include(p => p.EmployeHistory).ThenInclude(p => p.Gender)
                .Where(p=> p.EmployeId == id)
                .FirstOrDefaultAsync();

            return employe;
        }

        public string CreateParametr(Parameter parameter)
        {
            try
            {
                if (parameter == null) throw new ArgumentNullException(nameof(parameter));
                
                eComersContext.Parameters.Add(parameter);
                eComersContext.SaveChanges();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "0";
        } 



        public async Task<Parameter> GetParameterByName(string paramName)
        {
            Parameter parameter = new Parameter();
            
            if (eComersContext == null)
            {
                return new Parameter();
            }

            return await eComersContext.Parameters.Where(p => p.ParamName == paramName).FirstOrDefaultAsync()?? parameter;
        }

        public async Task<string> UpdateParameter(Parameter parameter)
        {
            try
            {
                if (parameter == null) throw new ArgumentNullException(nameof(parameter));

                eComersContext.Entry(parameter).State = EntityState.Modified;
                int rowCount = await eComersContext.SaveChangesAsync();
                return rowCount.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CreateCustomer(Customer customer)
        {
            try
            {
                if (customer == null) throw new ArgumentNullException(nameof(customer));

                eComersContext.Customer.Add(customer);
                eComersContext.SaveChanges();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "0";
        }



        public string RemoveAllCustomers()
        {
            int rowCount;
            try
            {
                rowCount = eComersContext.Database.ExecuteSqlRaw("DELETE FROM ecomers.\"Customer\" WHERE 1=1;");
                eComersContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return rowCount.ToString();
        }

        public async Task<List<Customer>> GetCustomersAsync() {
            List<Customer> listCustomers = new List<Customer>();
            try
            {
                listCustomers = await eComersContext.Customer.ToListAsync();
            }
            catch 
            {
                //TO DO SAVE LOG MESSAGE
            }

            return listCustomers;
        }
        public async Task<List<Customer>> GetCustomersByRootAsync(string root)
        {
            List<Customer> listCustomers = new List<Customer>();
            try
            {
                listCustomers = await eComersContext.Customer.Where(p=>p.U_RouteID == root).ToListAsync();
            }
            catch
            {
                //TO DO SAVE LOG MESSAGE
            }

            return listCustomers;
        }

        public async Task SaveOrderForSapServer(Order order)
        {
            try
            {
                if (order == null) 
                {
                    //TO DO SAVE LOG MESSAGE
                    return; 
                };
                eComersContext.Order.Add(order);
                await eComersContext.SaveChangesAsync();
            }
            catch (Exception e) 
            {
                var error = e.Message;
                //TO DO SAVE LOG MESSAGE
            }
        }

        public async Task SaveDeliveryForInvoice(DeliveryForInvoice deliveryForInvoice)
        {
            try
            {
                if (deliveryForInvoice == null)
                {
                    //TO DO SAVE LOG MESSAGE
                    return;
                };
                eComersContext.DeliveryForInvoice.Add(deliveryForInvoice);
                await eComersContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var error = e.Message;
                //TO DO SAVE LOG MESSAGE
            }
        }
    }
}
