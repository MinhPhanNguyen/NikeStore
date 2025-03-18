//document.addEventListener("DOMContentLoaded", function () {
//    // Hàm fetch dữ liệu và tạo biểu đồ
//    function fetchChartData(url, chartId, label, chartType = "bar") {
//        fetch(url)
//            .then(response => response.json())
//            .then(result => {
//                console.log(`Dữ liệu từ ${url}:`, result);
//                if (result && result.labels && result.data) {
//                    if (chartType === "bar") {
//                        createChart(chartId, result.labels, result.data, label);
//                    } else {
//                        createTodayChart(chartId, result.labels, result.data, label);
//                    }
//                } else {
//                    console.error(`Dữ liệu từ ${url} không hợp lệ`);
//                }
//            })
//            .catch(error => console.error(`Lỗi khi lấy dữ liệu từ ${url}:`, error));
//    }

//    // 🏆 FETCH DỮ LIỆU & TẠO BIỂU ĐỒ 🏆

//    // Biểu đồ doanh thu (Line Chart)
//    fetchChartData('/Admin/DashBoard/GetDailyRevenueData', 'dailyRevenueChart', 'Doanh thu ngày (triệu VNĐ)', "line");
//    fetchChartData('/Admin/DashBoard/GetMonthlyRevenueData', 'monthlyRevenueChart', 'Doanh thu tháng (triệu VNĐ)', "line");
//    fetchChartData('/Admin/DashBoard/GetYearRevenueData', 'yearRevenueChart', 'Doanh thu năm (triệu VNĐ)', "line");
//    fetchChartData('/Admin/DashBoard/GetDailyRevenueData', 'dailyRevenueChartPage', 'Doanh thu ngày (triệu VNĐ)', "line");
//    fetchChartData('/Admin/DashBoard/GetMonthlyRevenueData', 'monthlyRevenueChartPage', 'Doanh thu tháng (triệu VNĐ)', "line");
//    fetchChartData('/Admin/DashBoard/GetYearRevenueData', 'yearRevenueChartPage', 'Doanh thu năm (triệu VNĐ)', "line");
//    fetchChartData('/Admin/DashBoard/GetHourlyRevenueData', 'todayRevenueChart', 'Doanh thu giờ (triệu VNĐ)', "line");

//    // Biểu đồ sản phẩm (Bar Chart)
//    fetchChartData('/Admin/DashBoard/GetProductData', 'productChart', 'Sản phẩm bán chạy', "bar");
//    fetchChartData('/Admin/DashBoard/GetLoveProductData', 'loveProductChart', 'Sản phẩm yêu thích', "bar");
//    fetchChartData('/Admin/DashBoard/GetMostProductData', 'mostProductChart', 'Sản phẩm tìm kiếm nhiều nhất', "bar");
//    fetchChartData('/Admin/DashBoard/GetProductData', 'todayProductChart', 'Sản phẩm bán ra', "bar");
//    fetchChartData('/Admin/DashBoard/GetLoveProductData', 'loveProductChartPage', 'Sản phẩm yêu thích', "bar");
//    fetchChartData('/Admin/DashBoard/GetProductData', 'productChartPage', 'Sản phẩm bán chạy', "bar");
//    fetchChartData('/Admin/DashBoard/GetMostProductData', 'mostProductChartPage', 'Sản phẩm tìm kiếm nhiều nhất', "bar");

//    // Biểu đồ hóa đơn (Line Chart)
//    fetchChartData('/Admin/DashBoard/GetDailyOrderData', 'dailyOrderChart', 'Hóa đơn ngày', "line");
//    fetchChartData('/Admin/DashBoard/GetMonthlyOrderData', 'monthlyOrderChart', 'Hóa đơn tháng', "line");
//    fetchChartData('/Admin/DashBoard/GetYearOrderData', 'yearOrderChart', 'Hóa đơn năm', "line");

//    // Biểu đồ truy cập Website (Line Chart)
//    fetchChartData('/Admin/DashBoard/GetDailyAccessData', 'dailyAccessChart', 'Truy cập Website ngày', "line");
//    fetchChartData('/Admin/DashBoard/GetMonthlyAccessData', 'monthlyAccessChart', 'Truy cập Website tháng', "line");
//    fetchChartData('/Admin/DashBoard/GetYearAccessData', 'yearAccessChart', 'Truy cập Website năm', "line");
//});

//// 🏆 HÀM TẠO BAR CHART (Cột) 🏆
//function createChart(chartId, labels, data, label) {
//    const ctx = document.getElementById(chartId).getContext('2d');

//    const gradient = ctx.createLinearGradient(0, 0, 0, 400);
//    gradient.addColorStop(0, "#005eff");
//    gradient.addColorStop(1, "#00c6ff");

//    return new Chart(ctx, {
//        type: 'bar',
//        data: {
//            labels: labels,
//            datasets: [{
//                label: label,
//                data: data,
//                backgroundColor: gradient,
//                borderWidth: 1,
//                borderRadius: 4,
//                barPercentage: 0.7,
//            }]
//        },
//        options: {
//            responsive: true,
//            maintainAspectRatio: false,
//            plugins: {
//                legend: {
//                    display: true,
//                    position: "top",
//                    labels: { font: { size: 14 }, color: "#333" }
//                },
//                tooltip: {
//                    enabled: true,
//                    backgroundColor: "#333",
//                    titleFont: { size: 14 },
//                    bodyFont: { size: 12 }
//                }
//            },
//            scales: {
//                x: {
//                    grid: { display: false },
//                    ticks: { font: { size: 14 }, color: "#333" }
//                },
//                y: {
//                    beginAtZero: true,
//                    grid: { color: "#ddd", lineWidth: 0.5 },
//                    ticks: { font: { size: 14 }, color: "#333" }
//                }
//            }
//        }
//    });
//}

//// 🏆 HÀM TẠO LINE CHART (Đường) 🏆
//function createTodayChart(chartId, labels, data, label) {
//    const ctx = document.getElementById(chartId).getContext('2d');

//    return new Chart(ctx, {
//        type: 'line',
//        data: {
//            labels: labels,
//            datasets: [{
//                label: label,
//                data: data,
//                backgroundColor: "rgba(0, 94, 255, 0.2)",
//                borderColor: "#005eff",
//                borderWidth: 2,
//                pointBackgroundColor: "#005eff",
//                pointBorderColor: "#fff",
//                pointRadius: 5,
//                pointHoverRadius: 7,
//                tension: 0.4
//            }]
//        },
//        options: {
//            responsive: true,
//            maintainAspectRatio: false,
//            scales: {
//                x: {
//                    grid: { display: false },
//                    ticks: { font: { size: 14 } }
//                },
//                y: {
//                    beginAtZero: true,
//                    grid: { color: "#ddd" },
//                    ticks: { font: { size: 14 } }
//                }
//            },
//            plugins: {
//                legend: { display: true, position: "top" },
//                tooltip: { enabled: true }
//            }
//        }
//    });
//}
