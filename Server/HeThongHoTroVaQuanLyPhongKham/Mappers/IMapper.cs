namespace HeThongHoTroVaQuanLyPhongKham.Mappers
{
    public interface IMapper<TSource, TDestination>
    {
        TSource MapEntityToDto(TDestination entity);
        TDestination MapDtoToEntity(TSource dto);
        void MapDtoToEntity(TSource dto, TDestination entity);
    }
}
