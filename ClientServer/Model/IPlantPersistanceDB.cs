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
    public interface IPlantPersistanceDB
    {
        [OperationContract]
        List<Plant> loadPlants();

        [OperationContract]
        void savePlants(List<Plant> plants);

        [OperationContract]
        void savePlant(Plant plant);

        [OperationContract]
        void deletePlant(Plant plant);

        [OperationContract]
        Plant findPlant(string plant);

        [OperationContract]
        void editPlant(Plant oldPlant, Plant newPlant);

        [OperationContract]
        List<Plant> filterPlants(string criteria, string value);
    }
}
