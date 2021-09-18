namespace MMMouseAligner.Models
{
    public class History<T>
    {
        private readonly T[] historyArray;

        private int size;

        private int nextAdded = 0;

        private int lastAdded = -1;

        public History(int maxSize)
        {
            this.size = 0;
            this.historyArray = new T[maxSize];
        }

        public T this[int index]
            => this.Get(index);

        public void Enqueue(T item)
        {
            this.historyArray[this.nextAdded] = item;
            this.lastAdded = this.nextAdded;
            if (this.size < this.historyArray.Length)
            {
                this.size++;
            }

            this.nextAdded++;
            this.nextAdded %= this.historyArray.Length;
        }

        // Given size = 5 ; items = 1,2,3,4,5 ; index = -12 ; lastAdded = 4
        // 1: reduce the size of the index to be at max + or - size (% size)
        // => index = (-12 % 5) = -2
        // 2: add the size to make it positive
        // => index = (-2 + 5) = 3
        // 3: add lastAdded to shift the zero-point
        // => index = (3 + 4) = 7
        // 4: limit the number to the size (wrap around at the end => % size)
        // => index = (7 % 5) = 2
        public T Get(int index = 0)
            => this.historyArray[((index % this.size) + this.size + this.lastAdded) % this.size];
    }
}
