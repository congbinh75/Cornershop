import Card from './card';

const Dashboard = () => {
  return (
      <div className="grid grid-cols-1 gap-4 md:grid-cols-2 md:gap-6 xl:grid-cols-4 2xl:gap-7.5">
        <Card title="Total Orders" total="1000">
          <i className="fa-solid fa-cart-shopping"></i>
        </Card>
        <Card title="Total Revenue" total="4500">
          <i className="fa-solid fa-dollar-sign"></i>
        </Card>
        <Card title="Total Products" total="2450">
          <i className="fa-solid fa-book"></i>
        </Card>
        <Card title="Total Customers" total="3456">
          <i className="fa-solid fa-user"></i>
        </Card>
      </div>

  );
};

export default Dashboard;
