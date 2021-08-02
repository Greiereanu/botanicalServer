using Garden.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer.Model
{
    [ServiceContract]
    public interface IEmployeePersistanceDB
    {
        [OperationContract]
        List<Employee> loadEmployees();

        [OperationContract]
        void saveEmployees(List<Employee> employees);

        [OperationContract]
        void saveEmployee(Employee employee);

        [OperationContract]
        void deleteEmployee(Employee employee);

        [OperationContract]
        void editEmployee(Employee oldEmployee, Employee newEmployee);

        [OperationContract]
        Employee findEmployee(string account);

        [OperationContract]
        List<Employee> filterEmployees(string role);
    }
}
