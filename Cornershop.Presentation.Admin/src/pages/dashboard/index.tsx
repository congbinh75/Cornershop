import Card from "./components/card";

const Dashboard = () => {
  return (
    <div>
      <div className="grid grid-cols-1 gap-4 md:grid-cols-2 md:gap-6 xl:grid-cols-4 2xl:gap-7.5">
        <Card title="Total views" total="$3.456K" />
        <Card title="Total Profit" total="$45,2K" />
        <Card title="Total Product" total="2.450" />
        <Card title="Total Users" total="3.456" />
      </div>
    </div>
  );
};

export default Dashboard;
