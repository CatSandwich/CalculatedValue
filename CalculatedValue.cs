using System;

namespace CalculatedValue
{
    ///<summary>Holds a set of instructions to calculate a value of type <typeparamref name="T"/></summary>
    public class CalculatedValue<T> where T : IEquatable<T>
    {
        
        private readonly T _base;
        private readonly Layer<T>[] _layers;
        
        public ref Layer<T> this[int index] => ref _layers[index];

        ///<summary>Initializes layers and base value</summary>
        public CalculatedValue(byte layers, T @base){
            
           _layers = new Layer<T>[layers];
           for (var i = 0; i < _layers.Length; i++)
           {
               _layers[i] = new Layer<T>();
           }
            
           _base = @base;
        }

        ///<summary>Runs the base value through all the layers and returns the calculated value</summary>
        public T Calculate()
        {
            var value = _base;
            //foreach tested fasted
            foreach (var t in _layers)
            {
                //ref tested faster than return
                t.Calculate(ref value);
            }
            return value;
        }
    }

    ///<summary>A wrapper for a delegate that modifies a value. Callbacks added to an individual layer should not be dependent on order.</summary>
    public class Layer<T> where T : IEquatable<T>
    {
        public delegate void Sig(ref T value);

        private Sig _del = (ref T value) => { };
        
        private T _before;
        private T _after;
        private bool _isModified;
        
        public void Calculate(ref T value)
        {
            if (!_isModified && value.Equals(_before))
            {
                value = _after;
                return;
            }

            _before = value;
            _del(ref value);
            _after = value;
            _isModified = false;
        }

        public static Layer<T> operator +(Layer<T> layer, Sig del)
        {
            layer._del += del;
            layer._isModified = true;
            return layer;
        }

        public static Layer<T> operator -(Layer<T> layer, Sig del)
        {
            layer._del -= del;
            layer._isModified = true;
            return layer;
        }
    }
}