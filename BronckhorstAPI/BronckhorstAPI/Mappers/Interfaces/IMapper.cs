namespace BronckhorstAPI.Mappers.Interfaces;

public interface IMapper<in TEntity, out TDto>
{
    TDto MapToDto(TEntity entity);
}