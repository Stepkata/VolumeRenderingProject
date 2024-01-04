using System;
using System.Collections.Generic;
using System.Collections;

public class LongList<T>: IEnumerable<T>{
        private List<List<T>> Data;
        public int Count;
        private int index = 0;

        private long _maxLength = 500000;

        public LongList(){
            Data = new List<List<T>>
            {
                new()
            };
        }

        public LongList(long len){
            _maxLength = len;
            Data = new List<List<T>>
            {
                new()
            };
        }

        public void Add(T item){
            if (Data[index].Count > _maxLength){
                Data.Add(new List<T>());
                index++;
            }
            Data[index].Add(item);
            Count++;
        }

        public T At(int i){
            if (i >= Count){
                throw new Exception();
            }
            var ind = (int)(i/_maxLength);
            int at =(int)( i - ind*_maxLength);
            return Data[ind][at];
        }

        public List<List<T>> GetData(){
            return Data;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var sublist in Data)
            {
                foreach (var item in sublist)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
    }