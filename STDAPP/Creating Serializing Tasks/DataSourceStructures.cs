namespace STDAPP
{
    public class DataSourceStructures
    {
        public class DataGive // структура для передачи всех  данных в таск класс
        {
            public string ProductLink { get; set; }
            public string Size { get; set; }
            public string[] DropTime { get; set; }
            public string TimeMode { get; set; }
            public string[] Preload { get; set; }
            public string Account { get; set; }
            public string Proxy { get; set; }
            public ProfileClass Prof { get; set; }
            public int ChekoutMode { get; set; }
            public string CityToDns { get; set; }
            public int DnsModeToDns { get; set; }
            public string ShopIdToDns { get; set; }
            public int LamodaPaymentMode { get; set; }
            public int DnsPreload { get; set; }
            public string Cookies { get; set; }
            public string Headers { get; set; }
            public string Amount { get; set; }
            public int PsVersion { get; set; }
        }


        public struct DataEdit // структура для привязки к edit
        {
            public string ProductLink { get; set; }
            public string Size { get; set; }
            public string[] DropTime { get; set; }
            public int TimeMode { get; set; }
            public int Preload { get; set; }
            public int Account { get; set; }
            public int AccountChoose { get; set; }
            public int Proxy { get; set; }
            public int ProxyChoose { get; set; }
            public int Prof { get; set; }
            public int ChekoutMod { get; set; }
            public int Shop { get; set; }
            public string City { get; set; }
            public string ShopId { get; set; }
            public int DnsMode { get; set; }
            public int LamodaPayment { get; set; }
            public int DnsPreload { get; set; }
            public string Cookies { get; set; }
            public string Headers { get; set; }
            public string Amount { get; set; }
            public int PsVersion { get; set; }
        }

    }
}