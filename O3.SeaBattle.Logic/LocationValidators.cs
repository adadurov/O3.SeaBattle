namespace O3.SeaBattle.Logic
{
    public static class LocationValidators
    {
        public static IMatrixLocationValidator Default { get; } = new DefaultLocationValidator();
    }
}
