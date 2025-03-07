namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public interface IMapper<TSource, TDestination>
    {
        TSource MapEntityToDto(TDestination dto);
        TDestination MapDtoToEntity(TSource entity);
        void MapDtoToEntity(TSource dto, TDestination entity);
    }
}
