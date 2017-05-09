namespace SuperSimpleStocks.Utils
{
    public static class CalculationUtils
    {
        public static double CalculateGeometricMean(double[] inputValues)
        {
            return CalculateNRoot(inputValues.Length, CalculateProductFromValues(inputValues));
        }

        public static double CalculateProductFromValues(double[] inputValues)
        {
            double product = 1;
            foreach (var value in inputValues)
            {
                product = product * value;
            }
            return product;
        }

        public static double CalculateNRoot(double nForRoot, double inputVal)
        {
            return System.Math.Pow(inputVal, 1.0 / nForRoot);
        }
    }
}
