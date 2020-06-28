using CurrencyRateConverter.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRateConverter.Data
{
    public class RateDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public RateDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ConversionRate).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ConversionRate)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public Task<List<ConversionRate>> GetItemsAsync()
        {
            return Database.Table<ConversionRate>().ToListAsync();
        }

        public Task<List<ConversionRate>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<ConversionRate>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<ConversionRate> GetItemAsync(int id)
        {
            return Database.Table<ConversionRate>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(ConversionRate item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(ConversionRate item)
        {
            return Database.DeleteAsync(item);
        }

        /*internal<int> Task SaveItemAsync(ConversionRate item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }*/
    }
}
