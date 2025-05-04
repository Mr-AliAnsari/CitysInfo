namespace CitysInfo.Models
{
    /// <summary>
    /// شهر بدون نقاط دیدنی
    /// </summary>
    public class CityWithoutPointOfInterestDto
    {
        /// <summary>
        /// آیدی شهر
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// اسم شهر
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// توضیحات شهر
        /// </summary>
        public string? Description { get; set; }
    }
}
