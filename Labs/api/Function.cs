using System;


namespace Labs.api
{
    class Function<T, R>
    {
        T minXValue;
        T maxXValue;

        public T MinXValue
        {
            get { return minXValue; }
        }

        public T MaxXValue
        {
            get { return maxXValue; }
        }

        Func<T, R> innerFunction;

        public Function(T minXValue, T maxXValue, Func<T, R> function)
        {
            this.minXValue = minXValue;
            this.maxXValue = maxXValue;
            innerFunction = function;
        }

        public R execute(T x)
        {
            return innerFunction.Invoke(x);
        }
    }
}
