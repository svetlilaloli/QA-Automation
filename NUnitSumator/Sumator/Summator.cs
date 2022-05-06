namespace Summator
{
    public static class Summator
    {
        public static long Sum(int[] arr)
        {
            checked
            {
                long sum = 0;

                for (int i = 0; i < arr.Length; i++)
                {
                    sum += arr[i];
                }

                return sum;
            }
        }
        public static long Average(int[] arr)
        {
            checked
            {
                long sum = 0;

                for (int i = 0; i < arr.Length; i++)
                {
                    sum += arr[i];
                }

                return arr.Length == 0 ? 0 : sum / arr.Length;
            }
        }
    }
}
