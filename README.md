# PICalculator

## 依序單筆

### `static Random random = new Random(Guid.NewGuid().GetHashCode())`

#### Math.Pow(random.NextDouble(), 2) + Math.Pow(random.NextDouble(), 2)

| Pi Sample     | CPU | Memory | 總完成時間(ms) |
| ------------- | --- | ------ | -------------- |
| 100,000,000   | 24  | 117    | 7168           |
| 200,000,000   | 0   | 117    | 14294          |
| 300,000,000   | 0   | 119    | 21439          |
| 400,000,000   | 0   | 120    | 28495          |
| 500,000,000   | 0   | 121    | 35793          |
| 1,000,000,000 | 0   | 123    | 71533          |

```C#
static Random random = new Random(Guid.NewGuid().GetHashCode());
public static double Calculate(long sample)
{
    int sum = 0;
    for (int i = 0; i < sample; i++)
    {
        if (Math.Pow(random.NextDouble(), 2) + Math.Pow(random.NextDouble(), 2) < 1)
        {
           sum++;
        }
    }
    return 4.0 * sum / (sample);
}
```

#### x \* x + y \* y

| Pi Sample     | CPU | Memory | 總完成時間(ms) |
| ------------- | --- | ------ | -------------- |
| 100,000,000   | 0   | 0      | 2309           |
| 200,000,000   | 0   | 0      | 4403           |
| 300,000,000   | 0   | 0      | 6640           |
| 400,000,000   | 0   | 0      | 8839           |
| 500,000,000   | 0   | 0      | 10956          |
| 1,000,000,000 | 0   | 0      | 21975          |

```C#
static Random random = new Random(Guid.NewGuid().GetHashCode());
public static double Calculate(long sample)
{
    int sum = 0;
    for (int i = 0; i < sample; i++)
    {
        double x = random.NextDouble();
        double y = random.NextDouble();

        if (x * x + y * y < 1.0)
        {
            sum++;
        }
    }
    return 4.0 * sum / (sample);
}
```

# PICalculatorDotNet8

## Parallel 單筆

### `static Random random = new Random(Guid.NewGuid().GetHashCode())`

#### Math.Pow(random.NextDouble(), 2)

| Pi Sample     | CPU | Memory | 總完成時間(ms) |
| ------------- | --- | ------ | -------------- |
| 100,000,000   | 41  | 143    | 5401           |
| 200,000,000   | 55  | 145    | 10990          |
| 300,000,000   | 55  | 156    | 15716          |
| 400,000,000   | 57  | 157    | 21245          |
| 500,000,000   | 0   | 157    | 27202          |
| 1,000,000,000 | 0   | 163    | 53280          |

```C#
const int BATCH_QUANTITY = 2_500_000;

static Random random = new Random(Guid.NewGuid().GetHashCode());

public static async Task<double> Calculate(long sample)
{
    long BATCH = sample % BATCH_QUANTITY == 0 ? sample / BATCH_QUANTITY : sample / BATCH_QUANTITY + 1;
    long remainder = sample % BATCH_QUANTITY;


    long[] sumArray = new long[BATCH];
    long totalSum = 0;

    await Parallel.ForAsync(0, BATCH, (number, token) =>
    {
        int sum = 0;

        long quantity = BATCH_QUANTITY;
        if (number + 1 == BATCH && remainder != 0) quantity = remainder;

        for (int i = 0; i < quantity; i++)
        {
            if (Math.Pow(random.NextDouble(), 2) + Math.Pow(random.NextDouble(), 2) < 1)
            {
               sum++;
            }
        }

        sumArray[number] = sum;

        return ValueTask.CompletedTask;
    });

    for (int i = 0; i < sumArray.Length; i++) totalSum += sumArray[i];

    return 4.0 * totalSum / (sample);
}
```

#### x \* x + y \* y

| Pi Sample     | CPU | Memory | 總完成時間(ms) |
| ------------- | --- | ------ | -------------- |
| 100,000,000   | 41  | 143    | 5500           |
| 200,000,000   | 55  | 145    | 12121          |
| 300,000,000   | 55  | 156    | 18382          |
| 400,000,000   | 57  | 157    | 24403          |
| 500,000,000   | 0   | 157    | 30243          |
| 1,000,000,000 | 0   | 163    | 61746          |

```C#
const int BATCH_QUANTITY = 2_500_000;

static Random random = new Random(Guid.NewGuid().GetHashCode());

public static async Task<double> Calculate(long sample)
{
    long BATCH = sample % BATCH_QUANTITY == 0 ? sample / BATCH_QUANTITY : sample / BATCH_QUANTITY + 1;
    long remainder = sample % BATCH_QUANTITY;


    long[] sumArray = new long[BATCH];
    long totalSum = 0;

    await Parallel.ForAsync(0, BATCH, (number, token) =>
    {
        int sum = 0;

        long quantity = BATCH_QUANTITY;
        if (number + 1 == BATCH && remainder != 0) quantity = remainder;

        for (int i = 0; i < quantity; i++)
        {
            double x = random.NextDouble();
            double y = random.NextDouble();

            if (x * x + y * y < 1.0)
            {
                sum++;
            }
        }

        sumArray[number] = sum;

        return ValueTask.CompletedTask;
    });

    for (int i = 0; i < sumArray.Length; i++) totalSum += sumArray[i];

    return 4.0 * totalSum / (sample);
}
```

### `Random.Shared.NextDouble()`

#### x \* x + y \* y

| Pi Sample     | CPU | Memory | 總完成時間(ms) |
| ------------- | --- | ------ | -------------- |
| 100,000,000   | 0   | 0      | 867            |
| 200,000,000   | 0   | 0      | 871            |
| 300,000,000   | 0   | 0      | 1040           |
| 400,000,000   | 0   | 0      | 1210           |
| 500,000,000   | 0   | 0      | 1315           |
| 1,000,000,000 | 0   | 0      | 2670           |

```C#
const int BATCH_QUANTITY = 2_500_000;

public static async Task<double> Calculate(long sample)
{
    long BATCH = sample % BATCH_QUANTITY == 0 ? sample / BATCH_QUANTITY : sample / BATCH_QUANTITY + 1;
    long remainder = sample % BATCH_QUANTITY;

    long[] sumArray = new long[BATCH];
    long totalSum = 0;

    await Parallel.ForAsync(0, BATCH, (number, token) =>
    {
        int sum = 0;

        long quantity = BATCH_QUANTITY;
        if (number + 1 == BATCH && remainder != 0) quantity = remainder;

        for (int i = 0; i < quantity; i++)
        {
            double x = Random.Shared.NextDouble();
            double y = Random.Shared.NextDouble();
            if (x * x + y * y < 1.0)
            {
                sum++;
            }
        }

        sumArray[number] = sum;

        return ValueTask.CompletedTask;
    });

    for (int i = 0; i < sumArray.Length; i++) totalSum += sumArray[i];

    return 4.0 * totalSum / (sample);
}
```

### `Random.Shared.NextDouble()`

#### x \* x + y \* y

##### lock num

| Pi Sample   | CPU | Memory | 總完成時間(ms) |
| ----------- | --- | ------ | -------------- |
| 100,000,000 | 0   | 0      | 14601          |
| 200,000,000 | 0   | 0      | 26052          |

```C#
const int BATCH_QUANTITY = 2_500_000;

static object obj = new object();

public static async Task<double> Calculate(long sample)
{
    long BATCH = sample % BATCH_QUANTITY == 0 ? sample / BATCH_QUANTITY : sample / BATCH_QUANTITY + 1;
    long remainder = sample % BATCH_QUANTITY;

    int sum = 0;

    await Parallel.ForAsync(0, BATCH, (number, token) =>
    {

        long quantity = BATCH_QUANTITY;
        if (number + 1 == BATCH && remainder != 0) quantity = remainder;

        for (int i = 0; i < quantity; i++)
        {
            double x = Random.Shared.NextDouble();
            double y = Random.Shared.NextDouble();
            if (x * x + y * y < 1.0)
            {
                lock (obj) sum++;
            }
        }

        return ValueTask.CompletedTask;
    });

    return 4.0 * sum / (sample);
}
```

### `Random.Shared.NextDouble()`

#### x \* x + y \* y

##### Interlocked num

| Pi Sample     | CPU | Memory | 總完成時間(ms) |
| ------------- | --- | ------ | -------------- |
| 100,000,000   | 0   | 0      | 1574           |
| 200,000,000   | 0   | 0      | 1837           |
| 300,000,000   | 0   | 0      | 2097           |
| 400,000,000   | 0   | 0      | 2523           |
| 500,000,000   | 0   | 0      | 3702           |
| 1,000,000,000 | 0   | 0      | 5816           |

```C#
const int BATCH_QUANTITY = 2_500_000;

public static async Task<double> Calculate(long sample)
{
    long BATCH = sample % BATCH_QUANTITY == 0 ? sample / BATCH_QUANTITY : sample / BATCH_QUANTITY + 1;
    long remainder = sample % BATCH_QUANTITY;

    int sum = 0;

    await Parallel.ForAsync(0, BATCH, (number, token) =>
    {
        long quantity = BATCH_QUANTITY;
        if (number + 1 == BATCH && remainder != 0) quantity = remainder;

        int subTotal = 0;

        for (int i = 0; i < quantity; i++)
        {
            double x = Random.Shared.NextDouble();
            double y = Random.Shared.NextDouble();
            if (x * x + y * y < 1.0)
            {
                subTotal++;
            }
        }
        Interlocked.Add(ref sum, subTotal);

        return ValueTask.CompletedTask;
    });

    return 4.0 * sum / (sample);
}
```
