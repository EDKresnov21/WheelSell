using System.Collections.Generic;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL.Entities;

namespace WheelSellTA.BLL.DTO;

//Дата Трансфер Обджект, то есть Объект Трансфера Данных. Нужен, чтобы такие щекотливые вещи как пароль не хранились в БЛЛ
public class CarDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Price { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public string Location { get; set; }
    public int HorsePower { get; set; }
    public bool IsSold { get; set; }

    public int BrandId { get; set; }

    public int ModelId { get; set; }

    public int FuelTypeId { get; set; }

    public int TransmissionId { get; set; }

    public int OwnerId { get; set; }

    public ICollection<PhotoDTO> Photos { get; set; }
    public ICollection<VideoDTO> Videos { get; set; }
}