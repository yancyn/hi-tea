// 
//  ____  _     __  __      _        _ 
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from main on 2013-12-09 18:54:45Z.
// Please visit http://code.google.com/p/dblinq2007/ for more information.
//
using System;
using System.ComponentModel;
using System.Data;
#if MONO_STRICT
	using System.Data.Linq;
#else   // MONO_STRICT
	using DbLinq.Data.Linq;
	using DbLinq.Vendor;
#endif  // MONO_STRICT
	using System.Data.Linq.Mapping;
using System.Diagnostics;

namespace HiTea.Pos
{
    public partial class Main : DataContext
    {

        #region Extensibility Method Declarations
        partial void OnCreated();
        #endregion


        public Main(string connectionString) :
            base(connectionString)
        {
            this.OnCreated();
        }

        public Main(string connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            this.OnCreated();
        }

        public Main(IDbConnection connection, MappingSource mappingSource) :
            base(connection, mappingSource)
        {
            this.OnCreated();
        }

        public Table<Category> Categories
        {
            get
            {
                return this.GetTable<Category>();
            }
        }

        public Table<Charge> Charges
        {
            get
            {
                return this.GetTable<Charge>();
            }
        }

        public Table<Menu> Menus
        {
            get
            {
                return this.GetTable<Menu>();
            }
        }

        public Table<Order> Orders
        {
            get
            {
                return this.GetTable<Order>();
            }
        }

        public Table<OrderItem> OrderItems
        {
            get
            {
                return this.GetTable<OrderItem>();
            }
        }

        public Table<OrderSubItem> OrderSubItems
        {
            get
            {
                return this.GetTable<OrderSubItem>();
            }
        }

        public Table<OrderType> OrderTypes
        {
            get
            {
                return this.GetTable<OrderType>();
            }
        }

        public Table<Role> Roles
        {
            get
            {
                return this.GetTable<Role>();
            }
        }

        public Table<Status> Statuses
        {
            get
            {
                return this.GetTable<Status>();
            }
        }

        public Table<User> Users
        {
            get
            {
                return this.GetTable<User>();
            }
        }
    }

    #region Start MONO_STRICT
#if MONO_STRICT

public partial class Main
{
	
	public Main(IDbConnection connection) : 
			base(connection)
	{
		this.OnCreated();
	}
}
    #region End MONO_STRICT
#endregion
#else     // MONO_STRICT

    public partial class Main
    {

        public Main(IDbConnection connection) :
            base(connection, new DbLinq.Sqlite.SqliteVendor())
        {
            this.OnCreated();
        }

        public Main(IDbConnection connection, IVendor sqlDialect) :
            base(connection, sqlDialect)
        {
            this.OnCreated();
        }

        public Main(IDbConnection connection, MappingSource mappingSource, IVendor sqlDialect) :
            base(connection, mappingSource, sqlDialect)
        {
            this.OnCreated();
        }
    }
    #region End Not MONO_STRICT
    #endregion
#endif     // MONO_STRICT
    #endregion

    [Table(Name = "main.Category")]
    public partial class Category : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private string _name;

        private EntitySet<Menu> _menu;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);
        #endregion


        public Category()
        {
            _menu = new EntitySet<Menu>(new Action<Menu>(this.Menu_Attach), new Action<Menu>(this.Menu_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "Name", DbType = "varchar(30)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_menu", OtherKey = "CategoryID", ThisKey = "ID", Name = "fk_Menu_0")]
        [DebuggerNonUserCode()]
        public EntitySet<Menu> Menus
        {
            get
            {
                return this._menu;
            }
            set
            {
                this._menu = value;
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void Menu_Attach(Menu entity)
        {
            this.SendPropertyChanging();
            entity.Category = this;
        }

        private void Menu_Detach(Menu entity)
        {
            this.SendPropertyChanging();
            entity.Category = null;
        }
        #endregion
    }

    [Table(Name = "main.Charge")]
    public partial class Charge : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private bool _active;

        private int _id;

        private string _name;

        private float _value;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnActiveChanged();

        partial void OnActiveChanging(bool value);

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);

        partial void OnValueChanged();

        partial void OnValueChanging(float value);
        #endregion


        public Charge()
        {
            this.OnCreated();
        }

        [Column(Storage = "_active", Name = "Active", DbType = "bit", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public bool Active
        {
            get
            {
                return this._active;
            }
            set
            {
                if ((_active != value))
                {
                    this.OnActiveChanging(value);
                    this.SendPropertyChanging();
                    this._active = value;
                    this.SendPropertyChanged("Active");
                    this.OnActiveChanged();
                }
            }
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "Name", DbType = "varchar(30)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [Column(Storage = "_value", Name = "Value", DbType = "real", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public float Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if ((_value != value))
                {
                    this.OnValueChanging(value);
                    this.SendPropertyChanging();
                    this._value = value;
                    this.SendPropertyChanged("Value");
                    this.OnValueChanged();
                }
            }
        }

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table(Name = "main.Menu")]
    public partial class Menu : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private bool _active;

        private int _categoryID;

        private string _code;

        private string _description;

        private int _id;

        private string _image;

        private string _name;

        private float _price;

        private EntitySet<OrderItem> _orderItem;

        private EntitySet<OrderSubItem> _orderSubItem;

        private EntityRef<Category> _category = new EntityRef<Category>();

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnActiveChanged();

        partial void OnActiveChanging(bool value);

        partial void OnCategoryIDChanged();

        partial void OnCategoryIDChanging(int value);

        partial void OnCodeChanged();

        partial void OnCodeChanging(string value);

        partial void OnDescriptionChanged();

        partial void OnDescriptionChanging(string value);

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnImageChanged();

        partial void OnImageChanging(string value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);

        partial void OnPriceChanged();

        partial void OnPriceChanging(float value);
        #endregion


        public Menu()
        {
            _orderItem = new EntitySet<OrderItem>(new Action<OrderItem>(this.OrderItem_Attach), new Action<OrderItem>(this.OrderItem_Detach));
            _orderSubItem = new EntitySet<OrderSubItem>(new Action<OrderSubItem>(this.OrderSubItem_Attach), new Action<OrderSubItem>(this.OrderSubItem_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_active", Name = "Active", DbType = "bit", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public bool Active
        {
            get
            {
                return this._active;
            }
            set
            {
                if ((_active != value))
                {
                    this.OnActiveChanging(value);
                    this.SendPropertyChanging();
                    this._active = value;
                    this.SendPropertyChanged("Active");
                    this.OnActiveChanged();
                }
            }
        }

        [Column(Storage = "_categoryID", Name = "CategoryId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int CategoryID
        {
            get
            {
                return this._categoryID;
            }
            set
            {
                if ((_categoryID != value))
                {
                    this.OnCategoryIDChanging(value);
                    this.SendPropertyChanging();
                    this._categoryID = value;
                    this.SendPropertyChanged("CategoryID");
                    this.OnCategoryIDChanged();
                }
            }
        }

        [Column(Storage = "_code", Name = "Code", DbType = "text", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Code
        {
            get
            {
                return this._code;
            }
            set
            {
                if (((_code == value)
                            == false))
                {
                    this.OnCodeChanging(value);
                    this.SendPropertyChanging();
                    this._code = value;
                    this.SendPropertyChanged("Code");
                    this.OnCodeChanged();
                }
            }
        }

        [Column(Storage = "_description", Name = "Description", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (((_description == value)
                            == false))
                {
                    this.OnDescriptionChanging(value);
                    this.SendPropertyChanging();
                    this._description = value;
                    this.SendPropertyChanged("Description");
                    this.OnDescriptionChanged();
                }
            }
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_image", Name = "Image", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Image
        {
            get
            {
                return this._image;
            }
            set
            {
                if (((_image == value)
                            == false))
                {
                    this.OnImageChanging(value);
                    this.SendPropertyChanging();
                    this._image = value;
                    this.SendPropertyChanged("Image");
                    this.OnImageChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "Name", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [Column(Storage = "_price", Name = "Price", DbType = "real", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public float Price
        {
            get
            {
                return this._price;
            }
            set
            {
                if ((_price != value))
                {
                    this.OnPriceChanging(value);
                    this.SendPropertyChanging();
                    this._price = value;
                    this.SendPropertyChanged("Price");
                    this.OnPriceChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_orderItem", OtherKey = "MenuID", ThisKey = "ID", Name = "fk_OrderItem_1")]
        [DebuggerNonUserCode()]
        public EntitySet<OrderItem> OrderItem
        {
            get
            {
                return this._orderItem;
            }
            set
            {
                this._orderItem = value;
            }
        }

        [Association(Storage = "_orderSubItem", OtherKey = "MenuID", ThisKey = "ID", Name = "fk_OrderSubItem_0")]
        [DebuggerNonUserCode()]
        public EntitySet<OrderSubItem> OrderSubItem
        {
            get
            {
                return this._orderSubItem;
            }
            set
            {
                this._orderSubItem = value;
            }
        }
        #endregion

        #region Parents
        [Association(Storage = "_category", OtherKey = "ID", ThisKey = "CategoryID", Name = "fk_Menu_0", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Category Category
        {
            get
            {
                return this._category.Entity;
            }
            set
            {
                if (((this._category.Entity == value)
                            == false))
                {
                    if ((this._category.Entity != null))
                    {
                        Category previousCategory = this._category.Entity;
                        this._category.Entity = null;
                        previousCategory.Menus.Remove(this);
                    }
                    this._category.Entity = value;
                    if ((value != null))
                    {
                        value.Menus.Add(this);
                        _categoryID = value.ID;
                    }
                    else
                    {
                        _categoryID = default(int);
                    }
                }
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void OrderItem_Attach(OrderItem entity)
        {
            this.SendPropertyChanging();
            entity.Menu = this;
        }

        private void OrderItem_Detach(OrderItem entity)
        {
            this.SendPropertyChanging();
            entity.Menu = null;
        }

        private void OrderSubItem_Attach(OrderSubItem entity)
        {
            this.SendPropertyChanging();
            entity.Menu = this;
        }

        private void OrderSubItem_Detach(OrderSubItem entity)
        {
            this.SendPropertyChanging();
            entity.Menu = null;
        }
        #endregion
    }

    [Table(Name = "main.Order")]
    public partial class Order : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private System.DateTime _created;

        private int _createdByID;

        private System.Nullable<System.DateTime> _dodAte;

        private int _id;

        private int _memberID;

        private string _queueNo;

        private System.Nullable<System.DateTime> _receiptDate;

        private string _tableNo;

        private float _total;

        private EntitySet<OrderItem> _orderItem;

        private EntityRef<User> _user = new EntityRef<User>();

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnCreatedChanged();

        partial void OnCreatedChanging(System.DateTime value);

        partial void OnCreatedByIDChanged();

        partial void OnCreatedByIDChanging(int value);

        partial void OnDodAteChanged();

        partial void OnDodAteChanging(System.Nullable<System.DateTime> value);

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnMemberIDChanged();

        partial void OnMemberIDChanging(int value);

        partial void OnQueueNoChanged();

        partial void OnQueueNoChanging(string value);

        partial void OnReceiptDateChanged();

        partial void OnReceiptDateChanging(System.Nullable<System.DateTime> value);

        partial void OnTableNoChanged();

        partial void OnTableNoChanging(string value);

        partial void OnTotalChanged();

        partial void OnTotalChanging(float value);
        #endregion


        public Order()
        {
            _orderItem = new EntitySet<OrderItem>(new Action<OrderItem>(this.OrderItem_Attach), new Action<OrderItem>(this.OrderItem_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_created", Name = "Created", DbType = "datetime", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public System.DateTime Created
        {
            get
            {
                return this._created;
            }
            set
            {
                if ((_created != value))
                {
                    this.OnCreatedChanging(value);
                    this.SendPropertyChanging();
                    this._created = value;
                    this.SendPropertyChanged("Created");
                    this.OnCreatedChanged();
                }
            }
        }

        [Column(Storage = "_createdByID", Name = "CreatedById", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int CreatedByID
        {
            get
            {
                return this._createdByID;
            }
            set
            {
                if ((_createdByID != value))
                {
                    this.OnCreatedByIDChanging(value);
                    this.SendPropertyChanging();
                    this._createdByID = value;
                    this.SendPropertyChanged("CreatedByID");
                    this.OnCreatedByIDChanged();
                }
            }
        }

        [Column(Storage = "_dodAte", Name = "DODate", DbType = "datetime", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public System.Nullable<System.DateTime> DodAte
        {
            get
            {
                return this._dodAte;
            }
            set
            {
                if ((_dodAte != value))
                {
                    this.OnDodAteChanging(value);
                    this.SendPropertyChanging();
                    this._dodAte = value;
                    this.SendPropertyChanged("DodAte");
                    this.OnDodAteChanged();
                }
            }
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_queueNo", Name = "QueueNo", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string QueueNo
        {
            get
            {
                return this._queueNo;
            }
            set
            {
                if (((_queueNo == value)
                            == false))
                {
                    this.OnQueueNoChanging(value);
                    this.SendPropertyChanging();
                    this._queueNo = value;
                    this.SendPropertyChanged("QueueNo");
                    this.OnQueueNoChanged();
                }
            }
        }

        [Column(Storage = "_receiptDate", Name = "ReceiptDate", DbType = "datetime", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public System.Nullable<System.DateTime> ReceiptDate
        {
            get
            {
                return this._receiptDate;
            }
            set
            {
                if ((_receiptDate != value))
                {
                    this.OnReceiptDateChanging(value);
                    this.SendPropertyChanging();
                    this._receiptDate = value;
                    this.SendPropertyChanged("ReceiptDate");
                    this.OnReceiptDateChanged();
                }
            }
        }

        [Column(Storage = "_tableNo", Name = "TableNo", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string TableNo
        {
            get
            {
                return this._tableNo;
            }
            set
            {
                if (((_tableNo == value)
                            == false))
                {
                    this.OnTableNoChanging(value);
                    this.SendPropertyChanging();
                    this._tableNo = value;
                    this.SendPropertyChanged("TableNo");
                    this.OnTableNoChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_orderItem", OtherKey = "ParentID", ThisKey = "ID", Name = "fk_OrderItem_3")]
        [DebuggerNonUserCode()]
        public EntitySet<OrderItem> OrderItems
        {
            get
            {
                return this._orderItem;
            }
            set
            {
                this._orderItem = value;
            }
        }
        #endregion

        #region Parents
        [Association(Storage = "_user", OtherKey = "ID", ThisKey = "MemberID", Name = "fk_Order_0", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public User User
        {
            get
            {
                return this._user.Entity;
            }
            set
            {
                if (((this._user.Entity == value)
                            == false))
                {
                    if ((this._user.Entity != null))
                    {
                        User previousUser = this._user.Entity;
                        this._user.Entity = null;
                        previousUser.Order.Remove(this);
                    }
                    this._user.Entity = value;
                    if ((value != null))
                    {
                        value.Order.Add(this);
                        _memberID = value.ID;
                    }
                    else
                    {
                        _memberID = default(int);
                    }
                }
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void OrderItem_Attach(OrderItem entity)
        {
            this.SendPropertyChanging();
            entity.Order = this;
        }

        private void OrderItem_Detach(OrderItem entity)
        {
            this.SendPropertyChanging();
            entity.Order = null;
        }
        #endregion
    }

    [Table(Name = "main.OrderItem")]
    public partial class OrderItem : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private int _menuID;

        private int _orderTypeID;

        private int _parentID;

        private int _statusID;

        private EntitySet<OrderSubItem> _orderSubItem;

        private EntityRef<Status> _status = new EntityRef<Status>();

        private EntityRef<Menu> _menu = new EntityRef<Menu>();

        private EntityRef<OrderType> _orderType = new EntityRef<OrderType>();

        private EntityRef<Order> _order = new EntityRef<Order>();

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnMenuIDChanged();

        partial void OnMenuIDChanging(int value);

        partial void OnOrderTypeIDChanged();

        partial void OnOrderTypeIDChanging(int value);

        partial void OnParentIDChanged();

        partial void OnParentIDChanging(int value);

        partial void OnStatusIDChanged();

        partial void OnStatusIDChanging(int value);
        #endregion


        public OrderItem()
        {
            _orderSubItem = new EntitySet<OrderSubItem>(new Action<OrderSubItem>(this.OrderSubItem_Attach), new Action<OrderSubItem>(this.OrderSubItem_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_menuID", Name = "MenuId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int MenuID
        {
            get
            {
                return this._menuID;
            }
            set
            {
                if ((_menuID != value))
                {
                    this.OnMenuIDChanging(value);
                    this.SendPropertyChanging();
                    this._menuID = value;
                    this.SendPropertyChanged("MenuID");
                    this.OnMenuIDChanged();
                }
            }
        }

        [Column(Storage = "_orderTypeID", Name = "OrderTypeId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int OrderTypeID
        {
            get
            {
                return this._orderTypeID;
            }
            set
            {
                if ((_orderTypeID != value))
                {
                    this.OnOrderTypeIDChanging(value);
                    this.SendPropertyChanging();
                    this._orderTypeID = value;
                    this.SendPropertyChanged("OrderTypeID");
                    this.OnOrderTypeIDChanged();
                }
            }
        }

        [Column(Storage = "_parentID", Name = "ParentId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ParentID
        {
            get
            {
                return this._parentID;
            }
            set
            {
                if ((_parentID != value))
                {
                    this.OnParentIDChanging(value);
                    this.SendPropertyChanging();
                    this._parentID = value;
                    this.SendPropertyChanged("ParentID");
                    this.OnParentIDChanged();
                }
            }
        }

        [Column(Storage = "_statusID", Name = "StatusId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int StatusID
        {
            get
            {
                return this._statusID;
            }
            set
            {
                if ((_statusID != value))
                {
                    this.OnStatusIDChanging(value);
                    this.SendPropertyChanging();
                    this._statusID = value;
                    this.SendPropertyChanged("StatusID");
                    this.OnStatusIDChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_orderSubItem", OtherKey = "ParentID", ThisKey = "ID", Name = "fk_OrderSubItem_1")]
        [DebuggerNonUserCode()]
        public EntitySet<OrderSubItem> OrderSubItems
        {
            get
            {
                return this._orderSubItem;
            }
            set
            {
                this._orderSubItem = value;
            }
        }
        #endregion

        #region Parents
        [Association(Storage = "_status", OtherKey = "ID", ThisKey = "StatusID", Name = "fk_OrderItem_0", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Status Status
        {
            get
            {
                return this._status.Entity;
            }
            set
            {
                if (((this._status.Entity == value)
                            == false))
                {
                    if ((this._status.Entity != null))
                    {
                        Status previousStatus = this._status.Entity;
                        this._status.Entity = null;
                        previousStatus.OrderItem.Remove(this);
                    }
                    this._status.Entity = value;
                    if ((value != null))
                    {
                        value.OrderItem.Add(this);
                        _statusID = value.ID;
                    }
                    else
                    {
                        _statusID = default(int);
                    }
                }
            }
        }

        [Association(Storage = "_menu", OtherKey = "ID", ThisKey = "MenuID", Name = "fk_OrderItem_1", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Menu Menu
        {
            get
            {
                return this._menu.Entity;
            }
            set
            {
                if (((this._menu.Entity == value)
                            == false))
                {
                    if ((this._menu.Entity != null))
                    {
                        Menu previousMenu = this._menu.Entity;
                        this._menu.Entity = null;
                        previousMenu.OrderItem.Remove(this);
                    }
                    this._menu.Entity = value;
                    if ((value != null))
                    {
                        value.OrderItem.Add(this);
                        _menuID = value.ID;
                    }
                    else
                    {
                        _menuID = default(int);
                    }
                }
            }
        }

        [Association(Storage = "_orderType", OtherKey = "ID", ThisKey = "OrderTypeID", Name = "fk_OrderItem_2", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public OrderType OrderType
        {
            get
            {
                return this._orderType.Entity;
            }
            set
            {
                if (((this._orderType.Entity == value)
                            == false))
                {
                    if ((this._orderType.Entity != null))
                    {
                        OrderType previousOrderType = this._orderType.Entity;
                        this._orderType.Entity = null;
                        previousOrderType.OrderItem.Remove(this);
                    }
                    this._orderType.Entity = value;
                    if ((value != null))
                    {
                        value.OrderItem.Add(this);
                        _orderTypeID = value.ID;
                    }
                    else
                    {
                        _orderTypeID = default(int);
                    }
                }
            }
        }

        [Association(Storage = "_order", OtherKey = "ID", ThisKey = "ParentID", Name = "fk_OrderItem_3", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Order Order
        {
            get
            {
                return this._order.Entity;
            }
            set
            {
                if (((this._order.Entity == value)
                            == false))
                {
                    if ((this._order.Entity != null))
                    {
                        Order previousOrder = this._order.Entity;
                        this._order.Entity = null;
                        previousOrder.OrderItems.Remove(this);
                    }
                    this._order.Entity = value;
                    if ((value != null))
                    {
                        value.OrderItems.Add(this);
                        _parentID = value.ID;
                    }
                    else
                    {
                        _parentID = default(int);
                    }
                }
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void OrderSubItem_Attach(OrderSubItem entity)
        {
            this.SendPropertyChanging();
            entity.OrderItem = this;
        }

        private void OrderSubItem_Detach(OrderSubItem entity)
        {
            this.SendPropertyChanging();
            entity.OrderItem = null;
        }
        #endregion
    }

    [Table(Name = "main.OrderSubItem")]
    public partial class OrderSubItem : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private int _menuID;

        private int _parentID;

        private EntityRef<Menu> _menu = new EntityRef<Menu>();

        private EntityRef<OrderItem> _orderItem = new EntityRef<OrderItem>();

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnMenuIDChanged();

        partial void OnMenuIDChanging(int value);

        partial void OnParentIDChanged();

        partial void OnParentIDChanging(int value);
        #endregion


        public OrderSubItem()
        {
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_menuID", Name = "MenuId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int MenuID
        {
            get
            {
                return this._menuID;
            }
            set
            {
                if ((_menuID != value))
                {
                    this.OnMenuIDChanging(value);
                    this.SendPropertyChanging();
                    this._menuID = value;
                    this.SendPropertyChanged("MenuID");
                    this.OnMenuIDChanged();
                }
            }
        }

        [Column(Storage = "_parentID", Name = "ParentId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ParentID
        {
            get
            {
                return this._parentID;
            }
            set
            {
                if ((_parentID != value))
                {
                    this.OnParentIDChanging(value);
                    this.SendPropertyChanging();
                    this._parentID = value;
                    this.SendPropertyChanged("ParentID");
                    this.OnParentIDChanged();
                }
            }
        }

        #region Parents
        [Association(Storage = "_menu", OtherKey = "ID", ThisKey = "MenuID", Name = "fk_OrderSubItem_0", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Menu Menu
        {
            get
            {
                return this._menu.Entity;
            }
            set
            {
                if (((this._menu.Entity == value)
                            == false))
                {
                    if ((this._menu.Entity != null))
                    {
                        Menu previousMenu = this._menu.Entity;
                        this._menu.Entity = null;
                        previousMenu.OrderSubItem.Remove(this);
                    }
                    this._menu.Entity = value;
                    if ((value != null))
                    {
                        value.OrderSubItem.Add(this);
                        _menuID = value.ID;
                    }
                    else
                    {
                        _menuID = default(int);
                    }
                }
            }
        }

        [Association(Storage = "_orderItem", OtherKey = "ID", ThisKey = "ParentID", Name = "fk_OrderSubItem_1", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public OrderItem OrderItem
        {
            get
            {
                return this._orderItem.Entity;
            }
            set
            {
                if (((this._orderItem.Entity == value)
                            == false))
                {
                    if ((this._orderItem.Entity != null))
                    {
                        OrderItem previousOrderItem = this._orderItem.Entity;
                        this._orderItem.Entity = null;
                        previousOrderItem.OrderSubItems.Remove(this);
                    }
                    this._orderItem.Entity = value;
                    if ((value != null))
                    {
                        value.OrderSubItems.Add(this);
                        _parentID = value.ID;
                    }
                    else
                    {
                        _parentID = default(int);
                    }
                }
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Table(Name = "main.OrderType")]
    public partial class OrderType : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private string _name;

        private EntitySet<OrderItem> _orderItem;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);
        #endregion


        public OrderType()
        {
            _orderItem = new EntitySet<OrderItem>(new Action<OrderItem>(this.OrderItem_Attach), new Action<OrderItem>(this.OrderItem_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "Name", DbType = "varchar(30)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_orderItem", OtherKey = "OrderTypeID", ThisKey = "ID", Name = "fk_OrderItem_2")]
        [DebuggerNonUserCode()]
        public EntitySet<OrderItem> OrderItem
        {
            get
            {
                return this._orderItem;
            }
            set
            {
                this._orderItem = value;
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void OrderItem_Attach(OrderItem entity)
        {
            this.SendPropertyChanging();
            entity.OrderType = this;
        }

        private void OrderItem_Detach(OrderItem entity)
        {
            this.SendPropertyChanging();
            entity.OrderType = null;
        }
        #endregion
    }

    [Table(Name = "main.Role")]
    public partial class Role : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private string _name;

        private EntitySet<User> _user;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);
        #endregion


        public Role()
        {
            _user = new EntitySet<User>(new Action<User>(this.User_Attach), new Action<User>(this.User_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "Name", DbType = "varchar(15)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_user", OtherKey = "RoleID", ThisKey = "ID", Name = "fk_User_0")]
        [DebuggerNonUserCode()]
        public EntitySet<User> User
        {
            get
            {
                return this._user;
            }
            set
            {
                this._user = value;
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void User_Attach(User entity)
        {
            this.SendPropertyChanging();
            entity.Role = this;
        }

        private void User_Detach(User entity)
        {
            this.SendPropertyChanging();
            entity.Role = null;
        }
        #endregion
    }

    [Table(Name = "main.Status")]
    public partial class Status : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private int _id;

        private string _name;

        private EntitySet<OrderItem> _orderItem;

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnNameChanged();

        partial void OnNameChanging(string value);
        #endregion


        public Status()
        {
            _orderItem = new EntitySet<OrderItem>(new Action<OrderItem>(this.OrderItem_Attach), new Action<OrderItem>(this.OrderItem_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_name", Name = "Name", DbType = "varchar(30)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_orderItem", OtherKey = "StatusID", ThisKey = "ID", Name = "fk_OrderItem_0")]
        [DebuggerNonUserCode()]
        public EntitySet<OrderItem> OrderItem
        {
            get
            {
                return this._orderItem;
            }
            set
            {
                this._orderItem = value;
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void OrderItem_Attach(OrderItem entity)
        {
            this.SendPropertyChanging();
            entity.Status = this;
        }

        private void OrderItem_Detach(OrderItem entity)
        {
            this.SendPropertyChanging();
            entity.Status = null;
        }
        #endregion
    }

    [Table(Name = "main.User")]
    public partial class User : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {

        private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");

        private bool _active;

        private string _email;

        private string _ic;

        private string _displayname;

        private int _id;

        private string _mobile;

        private string _password;

        private int _point;

        private int _roleID;

        private string _username;

        private EntitySet<Order> _order;

        private EntityRef<Role> _role = new EntityRef<Role>();

        #region Extensibility Method Declarations
        partial void OnCreated();

        partial void OnActiveChanged();

        partial void OnActiveChanging(bool value);

        partial void OnCityChanged();

        partial void OnCityChanging(string value);

        partial void OnCountryChanged();

        partial void OnCountryChanging(string value);

        partial void OnDisplaynameChanged();

        partial void OnDisplaynameChanging(string value);

        partial void OnIDChanged();

        partial void OnIDChanging(int value);

        partial void OnMobileChanged();

        partial void OnMobileChanging(string value);

        partial void OnPasswordChanged();

        partial void OnPasswordChanging(string value);

        partial void OnPointChanged();

        partial void OnPointChanging(int value);

        partial void OnPostcodeChanged();

        partial void OnPostcodeChanging(string value);

        partial void OnRoleIDChanged();

        partial void OnRoleIDChanging(int value);

        partial void OnStateChanged();

        partial void OnStateChanging(string value);

        partial void OnStreet1Changed();

        partial void OnStreet1Changing(string value);

        partial void OnStreet2Changed();

        partial void OnStreet2Changing(string value);

        partial void OnTelephoneChanged();

        partial void OnTelephoneChanging(string value);

        partial void OnUsernameChanged();

        partial void OnUsernameChanging(string value);
        #endregion


        public User()
        {
            _order = new EntitySet<Order>(new Action<Order>(this.Order_Attach), new Action<Order>(this.Order_Detach));
            this.OnCreated();
        }

        [Column(Storage = "_active", Name = "Active", DbType = "bit", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public bool Active
        {
            get
            {
                return this._active;
            }
            set
            {
                if ((_active != value))
                {
                    this.OnActiveChanging(value);
                    this.SendPropertyChanging();
                    this._active = value;
                    this.SendPropertyChanged("Active");
                    this.OnActiveChanged();
                }
            }
        }

        [Column(Storage = "_email", Name = "Email", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                if (((_email == value)
                            == false))
                {
                    this.OnCityChanging(value);
                    this.SendPropertyChanging();
                    this._email = value;
                    this.SendPropertyChanged("Email");
                    this.OnCityChanged();
                }
            }
        }

        [Column(Storage = "_ic", Name = "Ic", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Ic
        {
            get
            {
                return this._ic;
            }
            set
            {
                if (((_ic == value)
                            == false))
                {
                    this.OnCountryChanging(value);
                    this.SendPropertyChanging();
                    this._ic = value;
                    this.SendPropertyChanged("Ic");
                    this.OnCountryChanged();
                }
            }
        }

        [Column(Storage = "_displayname", Name = "Displayname", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Displayname
        {
            get
            {
                return this._displayname;
            }
            set
            {
                if (((_displayname == value)
                            == false))
                {
                    this.OnDisplaynameChanging(value);
                    this.SendPropertyChanging();
                    this._displayname = value;
                    this.SendPropertyChanged("Displayname");
                    this.OnDisplaynameChanged();
                }
            }
        }

        [Column(Storage = "_id", Name = "Id", DbType = "integer", IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this.OnIDChanging(value);
                    this.SendPropertyChanging();
                    this._id = value;
                    this.SendPropertyChanged("ID");
                    this.OnIDChanged();
                }
            }
        }

        [Column(Storage = "_mobile", Name = "Mobile", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Mobile
        {
            get
            {
                return this._mobile;
            }
            set
            {
                if (((_mobile == value)
                            == false))
                {
                    this.OnMobileChanging(value);
                    this.SendPropertyChanging();
                    this._mobile = value;
                    this.SendPropertyChanged("Mobile");
                    this.OnMobileChanged();
                }
            }
        }

        [Column(Storage = "_password", Name = "Password", DbType = "text", AutoSync = AutoSync.Never)]
        [DebuggerNonUserCode()]
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (((_password == value)
                            == false))
                {
                    this.OnPasswordChanging(value);
                    this.SendPropertyChanging();
                    this._password = value;
                    this.SendPropertyChanged("Password");
                    this.OnPasswordChanged();
                }
            }
        }

        [Column(Storage = "_point", Name = "Point", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int Point
        {
            get
            {
                return this._point;
            }
            set
            {
                if ((_point != value))
                {
                    this.OnPointChanging(value);
                    this.SendPropertyChanging();
                    this._point = value;
                    this.SendPropertyChanged("Point");
                    this.OnPointChanged();
                }
            }
        }

        [Column(Storage = "_roleID", Name = "RoleId", DbType = "integer", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public int RoleID
        {
            get
            {
                return this._roleID;
            }
            set
            {
                if ((_roleID != value))
                {
                    this.OnRoleIDChanging(value);
                    this.SendPropertyChanging();
                    this._roleID = value;
                    this.SendPropertyChanged("RoleID");
                    this.OnRoleIDChanged();
                }
            }
        }

        [Column(Storage = "_username", Name = "Username", DbType = "varchar(30)", AutoSync = AutoSync.Never, CanBeNull = false)]
        [DebuggerNonUserCode()]
        public string Username
        {
            get
            {
                return this._username;
            }
            set
            {
                if (((_username == value)
                            == false))
                {
                    this.OnUsernameChanging(value);
                    this.SendPropertyChanging();
                    this._username = value;
                    this.SendPropertyChanged("Username");
                    this.OnUsernameChanged();
                }
            }
        }

        #region Children
        [Association(Storage = "_order", OtherKey = "MemberID", ThisKey = "ID", Name = "fk_Order_0")]
        [DebuggerNonUserCode()]
        public EntitySet<Order> Order
        {
            get
            {
                return this._order;
            }
            set
            {
                this._order = value;
            }
        }
        #endregion

        #region Parents
        [Association(Storage = "_role", OtherKey = "ID", ThisKey = "RoleID", Name = "fk_User_0", IsForeignKey = true)]
        [DebuggerNonUserCode()]
        public Role Role
        {
            get
            {
                return this._role.Entity;
            }
            set
            {
                if (((this._role.Entity == value)
                            == false))
                {
                    if ((this._role.Entity != null))
                    {
                        Role previousRole = this._role.Entity;
                        this._role.Entity = null;
                        previousRole.User.Remove(this);
                    }
                    this._role.Entity = value;
                    if ((value != null))
                    {
                        value.User.Add(this);
                        _roleID = value.ID;
                    }
                    else
                    {
                        _roleID = default(int);
                    }
                }
            }
        }
        #endregion

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
            if ((h != null))
            {
                h(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
            if ((h != null))
            {
                h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #region Attachment handlers
        private void Order_Attach(Order entity)
        {
            this.SendPropertyChanging();
            entity.User = this;
        }

        private void Order_Detach(Order entity)
        {
            this.SendPropertyChanging();
            entity.User = null;
        }
        #endregion
    }
}