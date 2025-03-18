using System.Linq;
using NikeStore.Models;
using Microsoft.EntityFrameworkCore;

namespace NikeStore.Repository
{
    public class SeedData
    {
        public static void SeedDatabase(DataContext _context)
        {
            _context.Database.Migrate();

            // Add Product Categories (Collections) if they don't exist
            if (!_context.Warehouse.Any()) // Kiểm tra xem có dữ liệu chưa
            {
                _context.Warehouse.AddRange(
                    new Warehouse
                    {
                        WarehouseName = "Main Warehouse",
                        Location = "Hanoi, Vietnam",
                        Capacity = 10000,
                        CreatedAt = DateTime.Now
                    },
                    new Warehouse
                    {
                        WarehouseName = "Backup Warehouse",
                        Location = "Ho Chi Minh, Vietnam",
                        Capacity = 5000,
                        CreatedAt = DateTime.Now
                    }
                );
                _context.SaveChanges(); // Lưu dữ liệu vào DB
            }

            if (!_context.Promotion.Any()) // Kiểm tra xem có dữ liệu chưa
            {
                _context.Promotion.AddRange(
                    new Promotion
                    {
                        Discount = 10,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(1),
                        IsActive = true,
                    },
                    new Promotion
                    {
                        Discount = 20,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(1),
                        IsActive = true,
                    }
                );
                _context.SaveChanges(); // Lưu dữ liệu vào DB
            }

            // Add Product Categories (Collections) if they don't exist
            if (!_context.Provider.Any()) // Kiểm tra xem có dữ liệu chưa
            {
                _context.Provider.AddRange(
                   new Provider
                   {
                       ProviderName = "Nike Supplier",
                       ContactPerson = "John Doe",
                       Phone = "0123456789",
                       Email = "supplier@nike.com",
                       Address = "USA",
                       CreatedAt = DateTime.Now
                   },
                    new Provider
                    {
                        ProviderName = "Adidas Supplier",
                        ContactPerson = "Jane Smith",
                        Phone = "0987654321",
                        Email = "supplier@adidas.com",
                        Address = "Germany",
                        CreatedAt = DateTime.Now
                    }
                );
                _context.SaveChanges(); // Lưu dữ liệu vào DB
            }

            // Add Product Categories (Collections) if they don't exist
            if (!_context.ProductCategory.Any())
            {
                _context.ProductCategory.AddRange(
                    new ProductCategory
                    {
                        CategoryName = "Air Force 1",
                        Slug = "airforce1",
                        Description = "Bộ sưu tập Air Force 1",
                        ImageUrl = "1.png"
                    },
                    new ProductCategory
                    {
                        CategoryName = "Air Jordan 1",
                        Slug = "airjordan1",
                        Description = "Bộ sưu tập Air Jordan 1",
                        ImageUrl = "2.png"
                    },
                     new ProductCategory
                     {
                         CategoryName = "Air Max",
                         Slug = "airmax",
                         Description = "Bộ sưu tập Air Max",
                         ImageUrl = "3.png"
                     },
                    new ProductCategory
                    {
                        CategoryName = "Dunk",
                        Slug = "dunk",
                        Description = "Bộ sưu tập Dunk",
                        ImageUrl = "4.png",
                    },
                    new ProductCategory
                    {
                        CategoryName = "Blazer",
                        Slug = "blazer",
                        Description = "Bộ sưu tập Blazer",
                        ImageUrl = "5.png"
                    },
                    new ProductCategory
                    {
                        CategoryName = "Pegasus",
                        Slug = "pegasus",
                        Description = "Bộ sưu tập Pegasus",
                        ImageUrl = "6.png"
                    }
                );
                _context.SaveChanges(); // Save categories to ensure IDs are generated
            }

            // Retrieve category IDs after saving them
            var airForceCategory = _context.ProductCategory.FirstOrDefault(c => c.CategoryName == "Air Force 1");
            var jordanCategory = _context.ProductCategory.FirstOrDefault(c => c.CategoryName == "Air Jordan 1");
            var maxCategory = _context.ProductCategory.FirstOrDefault(c => c.CategoryName == "Air Max");
            var dunkCategory = _context.ProductCategory.FirstOrDefault(c => c.CategoryName == "Dunk");
            var blazer = _context.ProductCategory.FirstOrDefault(c => c.CategoryName == "Blazer");
            var pegasus = _context.ProductCategory.FirstOrDefault(c => c.CategoryName == "Pegasus");

            if (!_context.Service.Any())
            {
                _context.Service.AddRange(
                    new Service { Name = "DỊCH VỤ SAU BÁN HÀNG", Description = "Các dịch vụ hỗ trợ sau khi mua hàng.", ImageUrl = "1.png" },
                    new Service { Name = "GIÀY VÀ PHỤ KIỆN", Description = "Các dịch vụ hỗ trợ sau khi mua hàng.", ImageUrl = "2.png" },
                    new Service { Name = "HỖ TRỢ KHÁCH HÀNG", Description = "Các dịch vụ hỗ trợ sau khi mua hàng.", ImageUrl = "3.png" },
                    new Service { Name = "ĐẶT LỊCH HẸN", Description = "Các dịch vụ hỗ trợ sau khi mua hàng.", ImageUrl = "4.png" }
                );
                _context.SaveChanges(); // Save categories to ensure IDs are generated
            }

            if (!_context.ServiceType.Any())
            {
                _context.ServiceType.AddRange(
                    new ServiceType { Name = "Chính sách bảo hành", Description = "Chính sách bảo hành chính hãng.", ServiceID = 1, Link = "/Service/Index" },
                    new ServiceType { Name = "Dịch vụ sửa chữa", Description = "Chính sách bảo hành chính hãng.", ServiceID = 1, Link = "/Repairing/Index" },
                    new ServiceType { Name = "Dịch vụ chăm sóc giày", Description = "<p>Để trải nghiệm mua sắm và sử dụng sản phẩm của quý khách luôn an tâm và hài lòng, Nike luôn đồng hành cùng quý khách mọi lúc mọi nơi thông qua tổng đài chăm sóc khách hàng (hay còn gọi là Contact Center).</p>       <img src='/images/Caring/20210630164423.jpg' alt='img' />       <p>Bắt đầu hoạt động từ năm 2015, Contact Center của Nike đã đồng hành và phục vụ nhiều khách hàng qua việc tiếp nhận các cuộc gọi và cung cấp các thông tin về sản phẩm, dịch vụ, chương trình ưu đãi và thông tin liên hệ của các cửa hàng Nike trên toàn quốc.</p>       <p>Khi quý khách gọi đến tổng đài 1800 1234, đội ngũ nhân viên chăm sóc khách hàng của chúng tôi sẽ tiếp nhận các cuộc gọi, tư vấn giải đáp các yêu cầu thắc mắc, hỗ trợ khách hàng về: tư vấn sản phẩm (giá cả, chương trình ưu đãi); thông tin về bảo hành, bảo dưỡng, các chương trình hỗ trợ khách hàng; giải đáp thắc mắc khác liên quan đến các cửa hàng Nike trên toàn quốc, với quy trình xử lý chuyên nghiệp.</p>       <p>Thời gian làm việc của Contact Center từ 8h – 17h (Thứ 2 – Thứ 6) và từ 8h – 12h hoặc 17h (Thứ 7). Tất cả các cuộc gọi ngoài giờ làm việc, nếu quý khách cần hỗ trợ khẩn cấp, có thể lựa chọn chuyển tiếp cuộc gọi đến Bộ phận Kinh doanh hoặc Bộ phận Dịch vụ của Nike. Chúng tôi cam kết bảo mật tất cả thông tin khách hàng chia sẻ qua điện thoại.</p>", ServiceID = 1, Link = "/Caring/Index" },
                    new ServiceType { Name = "Giày chính hãng", Description = "<p>Để đảm bảo giày Nike của bạn luôn hoạt động hiệu quả nhất, việc bảo dưỡng định kỳ với phụ tùng chính hãng là rất quan trọng. Mỗi bộ phận của giày đóng vai trò then chốt trong hiệu suất và độ bền của giày. Lựa chọn các phụ tùng chính hãng giúp kéo dài tuổi thọ giày và mang lại sự thoải mái tối ưu khi sử dụng.</p>       <h2>1. Thay đế giày</h2>       <p>Đế giày đóng vai trò quan trọng trong việc cung cấp độ bám, sự hỗ trợ và cảm giác thoải mái. Bạn nên thay đế giày sau <strong>5,000 km hoặc 3 tháng</strong> để duy trì hiệu suất tốt nhất và tránh gây khó chịu khi đi hoặc chạy.</p>       <h3>Lợi ích của việc thay đế giày:</h3>       <p>- Cung cấp độ bám và sự ổn định.</p>       <p>- Tăng cường sự thoải mái khi di chuyển.</p>       <p>- Giảm nguy cơ chấn thương do hỗ trợ không đúng cách.</p>       <p>- Kéo dài tuổi thọ của giày.</p>      <h2>2. Dây giày</h2>       <p>Dây giày bị hư hỏng có thể gây khó chịu hoặc ảnh hưởng đến hiệu suất. Nên thay dây giày sau <strong>6 tháng hoặc 1,000 km</strong> sử dụng để đảm bảo giày luôn vừa vặn và an toàn.</p>       <p>Hậu quả của việc không thay dây giày:</p>       <p>- Giảm độ vừa vặn và thoải mái.</p>       <p>- Tăng nguy cơ chấn thương do không hỗ trợ chân đúng cách.</p>      <h2>3. Lót giày</h2>       <h3>a. Lót giày hiệu suất cao</h3>       <p>Lót giày hiệu suất cao cung cấp thêm đệm và hỗ trợ cho các hoạt động như chạy hoặc đi bộ lâu. Nên thay mới sau <strong>6 tháng hoặc 1,000 km</strong>.</p>      <h3>b. Lót giày dùng hàng ngày</h3>       <p>Lót giày này mang lại sự thoải mái cho việc sử dụng hàng ngày. Nên thay mới sau <strong>1 năm hoặc 2,000 km</strong> để duy trì sự hỗ trợ cho đôi chân.</p>      <h3>c. Lót giày chỉnh hình</h3>       <p>Dành cho những ai có vấn đề về chân, lót giày chỉnh hình giúp hỗ trợ chân tốt hơn. Nên thay mới sau <strong>1 năm hoặc 2,500 km</strong> để duy trì sự thoải mái.</p>      <h2>4. Chăm sóc phần trên của giày</h2>       <p>Phần trên của giày sẽ bị mòn theo thời gian, đặc biệt là ở các khu vực tiếp xúc nhiều. Nên vệ sinh và kiểm tra thường xuyên. Thay thế khi cần thiết sau <strong>6 tháng hoặc 1,000 km</strong> sử dụng để giày luôn trong tình trạng tốt nhất.</p>      <h2>5. Hệ thống đệm Nike Air</h2>       <p>Hệ thống đệm Nike Air giúp giảm chấn và mang lại sự thoải mái. Bạn cần kiểm tra hệ thống đệm và thay thế khi cảm thấy đệm giảm hiệu suất sau <strong>1-2 năm hoặc 1,500 km</strong> sử dụng.</p>      <h2>6. Củng cố phần gót giày</h2>       <p>Gót giày có thể bị mất hình dạng theo thời gian. Nếu bạn nhận thấy gót giày bị lỏng hoặc mất cấu trúc, nên thay thế sau <strong>6 tháng hoặc 800 km</strong> sử dụng để tránh gây khó chịu khi đi bộ.</p>      <h2>7. Công nghệ Nike Zoom</h2>       <p>Đối với giày Nike Zoom, đệm phản ứng đóng vai trò quan trọng trong hiệu suất. Nếu bạn cảm thấy giày không còn độ phản hồi như trước, hãy thay thế đệm Zoom sau <strong>1 năm hoặc 1,500 km</strong> để duy trì lợi ích về hiệu suất.</p>      <h2>8. Chăm sóc chống thấm nước</h2>       <p>Đối với những giày sử dụng trong điều kiện ẩm ướt, việc chăm sóc chống thấm nước là rất cần thiết. Nên áp dụng lại chất chống thấm mỗi <strong>6 tháng</strong> để giữ cho giày luôn khô ráo và bảo vệ giày khỏi các tác nhân bên ngoài.</p>      <h2>9. Vệ sinh và bảo dưỡng giày</h2>       <p>Vệ sinh và bảo dưỡng đúng cách giúp giày kéo dài tuổi thọ. Nên vệ sinh giày sau mỗi <strong>100 km</strong> sử dụng và xử lý kịp thời các vết bẩn, vết ố để bảo vệ giày.</p>      <h2>10. Cách bảo quản giày</h2>       <p>Bảo quản giày đúng cách khi không sử dụng sẽ tránh được hư hỏng. Dùng cây giữ giày để duy trì hình dạng và cất giày ở nơi khô ráo, thoáng mát để tránh giày bị hỏng.", ServiceID = 2, Link = "/AuthShoe/Index" },
                    new ServiceType { Name = "Phụ kiện chính hãng", Description = "   <p>Để đảm bảo đôi giày Nike của bạn luôn bền đẹp và thoải mái khi sử dụng, việc bảo dưỡng và chọn lựa giày chính hãng là điều rất quan trọng. Mỗi đôi giày được thiết kế để mang lại sự thoải mái tối đa và nâng cao hiệu suất vận động, do đó việc lựa chọn giày đúng kích cỡ và chất liệu phù hợp sẽ giúp bạn trải nghiệm tốt nhất.</p>       <h2>1. Giày chạy bộ Nike Air Zoom</h2>       <p>Giày chạy bộ Nike Air Zoom giúp bạn cảm nhận sự thoải mái trong từng bước chạy, hỗ trợ giảm chấn động và tăng hiệu suất. Nên thay giày khi lớp đế bị mòn hoặc sau <strong>500 km</strong> sử dụng để duy trì độ bền và tính năng vận động tối ưu.</p>       <h3>Công dụng của giày chạy bộ Nike Air Zoom:</h3>       <p>- Đệm Nike Zoom giúp tăng khả năng phản hồi và giảm thiểu lực tác động lên chân.</p>       <p>- Thiết kế thoáng khí giúp đôi chân luôn khô ráo và thoải mái.</p>       <p>- Tăng cường hỗ trợ cho bàn chân và cổ chân trong quá trình chạy.</p>       <p>- Chống trơn trượt và bám dính tuyệt vời.</p>       <h2>2. Giày thể thao Nike Air Max</h2>       <p>Giày thể thao Nike Air Max mang đến cảm giác êm ái và thoải mái trong suốt cả ngày dài. Nên thay giày khi đế giày bị mòn hoặc sau <strong>400 km</strong> sử dụng để đảm bảo sự thoải mái khi di chuyển.</p>       <p>Hậu quả khi không thay giày đúng thời điểm:</p>       <p>- Giày mất khả năng hỗ trợ, gây đau mỏi khi di chuyển.</p>       <p>- Đế giày mòn làm giảm độ bám và sự ổn định khi vận động.</p>       <p>- Mất tính năng bảo vệ đôi chân khỏi chấn thương.</p>       <h2>3. Các phụ kiện giày Nike</h2>       <h3>a. Miếng lót giày</h3>       <p>Miếng lót giày giúp tạo sự thoải mái và giảm áp lực lên đôi chân. Nên thay miếng lót giày sau mỗi <strong>6 tháng</strong> sử dụng.</p>       <h3>b. Dây giày</h3>       <p>Dây giày giúp điều chỉnh độ vừa vặn, nên thay mới khi dây giày bị rách hoặc không còn bền.</p>       <h3>c. Túi đựng giày</h3>       <p>Túi đựng giày giúp bảo vệ giày khi vận chuyển. Nên sử dụng túi đựng khi mang giày đi du lịch hoặc khi không sử dụng giày trong thời gian dài.</p>       <h3>d. Phụ kiện chống mùi giày</h3>       <p>Phụ kiện này giúp giữ cho đôi giày luôn thơm tho và khô ráo, có thể thay thế mỗi <strong>3 tháng</strong>.</p>       <h2>4. Giày bóng rổ Nike LeBron</h2>       <p>Giày bóng rổ Nike LeBron mang đến sự ổn định và khả năng kiểm soát tuyệt vời. Nên thay giày sau <strong>400 km</strong> chơi bóng rổ để giữ được độ bám và sự hỗ trợ tốt nhất cho bàn chân.</p>       <h2>5. Giày đi bộ Nike Free</h2>       <p>Giày đi bộ Nike Free với thiết kế linh hoạt và đệm Nike giúp bạn di chuyển tự nhiên. Nên thay giày khi đế bị mòn hoặc sau <strong>500 km</strong> sử dụng.</p>       <h2>6. Giày đá bóng Nike Mercurial</h2>       <p>Giày đá bóng Nike Mercurial giúp bạn tăng tốc và bứt phá trong mỗi trận đấu. Nên thay giày khi đế giày bị mòn hoặc sau <strong>300 km</strong> chơi bóng.</p>       <h2>7. Giày Nike Air Force 1</h2>       <p>Giày Nike Air Force 1 với phong cách cổ điển và bền bỉ, nên thay giày khi đế bị mòn hoặc sau <strong>400 km</strong> sử dụng để duy trì sự thoải mái và độ bền.</p>       <h2>8. Giày Nike Pegasus</h2>       <p>Giày Nike Pegasus là sự lựa chọn tuyệt vời cho những ai yêu thích chạy bộ. Nên thay giày sau mỗi <strong>500 km</strong> để đảm bảo giày vẫn hỗ trợ tốt cho đôi chân trong suốt quá trình vận động.</p>       <h2>9. Giày Nike ZoomX</h2>       <p>Giày Nike ZoomX giúp tăng hiệu suất chạy và tạo cảm giác nhẹ nhàng. Nên thay giày khi lớp đế giày bị mòn hoặc sau <strong>500 km</strong> sử dụng.</p>       <h2>10. Dung dịch làm sạch giày</h2>       <p>Dung dịch làm sạch giúp giày luôn mới mẻ và bền lâu. Nên vệ sinh giày sau mỗi lần sử dụng hoặc ít nhất <strong>1 tháng 1 lần</strong> để giữ giày luôn sạch sẽ.</p>", ServiceID = 2, Link = "/AuthAccessories/Index" },
                    new ServiceType { Name = "Tổng đài chăm sóc khách hàng", Description = " <p>Giày Nike của bạn sẽ luôn giữ được độ bền, vẻ đẹp và sự thoải mái khi được bảo dưỡng và chăm sóc định kỳ. Chúng tôi cung cấp các dịch vụ chăm sóc giày chính hãng với những quy trình chuyên nghiệp giúp bảo vệ giày khỏi bụi bẩn, vết ố và các tác nhân gây hại từ môi trường. Đến với dịch vụ chăm sóc giày Nike, bạn sẽ luôn cảm nhận được sự khác biệt với những đôi giày như mới.</p>      <h2>DỊCH VỤ VỆ SINH GIÀY CAO CẤP</h2>       <p>Chúng tôi sử dụng công nghệ tiên tiến để làm sạch giày Nike của bạn, loại bỏ bụi bẩn, vết ố và các vết bẩn cứng đầu mà không làm hỏng chất liệu. Quy trình vệ sinh giúp duy trì độ sáng bóng của giày, bảo vệ bề mặt da và vải, giúp đôi giày của bạn luôn trông như mới.</p>       <img src='/images/Caring/banner-ve-sinh-giay-mobile-goc.jpg' alt='img1' />      <h2>DỊCH VỤ TẨY TRẮNG GIÀY</h2>       <p>Dịch vụ tẩy trắng giày giúp làm sạch vết bẩn, vết ố trên các đôi giày trắng, trả lại vẻ đẹp sáng bóng. Chúng tôi sử dụng các chất tẩy an toàn, không gây hại cho giày mà vẫn đảm bảo giày luôn mới và đẹp như lần đầu.</p>       <img src='/images/Caring/cach-giat-giay-vai.jpg' alt='img2' />      <h2>DỊCH VỤ PHỤC HỒI BỀ MẶT GIÀY</h2>       <p>Dịch vụ này giúp phục hồi bề mặt giày bị trầy xước hoặc bị bẩn do tác động từ môi trường. Giày sẽ được làm mới lại, với chất liệu được bảo vệ, giúp duy trì vẻ đẹp và độ bền lâu dài của sản phẩm.</p>       <img src='/images/Caring/dan-bao-ve-de-giay-sneaker-3.jpg' alt='img3' />      <h2>DỊCH VỤ KIỂM TRA VÀ BẢO DƯỠNG ĐẾ GIÀY</h2>       <p>Chúng tôi cung cấp dịch vụ kiểm tra và bảo dưỡng đế giày, giúp tăng cường độ bám và độ bền của đế giày, đảm bảo giày luôn vận hành tốt trong mọi điều kiện thời tiết và trên mọi loại bề mặt.</p>       <img src='/images/Caring/dich-vu-ve-sinh-giay-washintown.jpg' alt='img4' />      <h2>DỊCH VỤ BẢO DƯỠNG VẢI GIÀY</h2>       <p>Dịch vụ này giúp bảo vệ các loại vải giày như mesh, vải tổng hợp khỏi sự mài mòn và các tác nhân gây hại từ môi trường. Chúng tôi sẽ giúp giày luôn giữ được độ thoáng khí và sự thoải mái cho người sử dụng.</p>       <img src='/images/Caring/Heramo-Noi-cung-cap-dich-vu-sua.jpg' alt='img5' />      <h2>DỊCH VỤ PHỦ BẢO VỆ BỀ MẶT GIÀY</h2>       <p>Với dịch vụ phủ bảo vệ giày, chúng tôi sẽ bảo vệ giày của bạn khỏi bụi bẩn, nước và các chất bẩn khác, giúp duy trì vẻ đẹp của giày và bảo vệ giày khỏi bị phai màu, rách hoặc biến dạng trong quá trình sử dụng.</p>       <img src='/images/Caring/psd-wit-poster-2692.jpg' alt='img5' />      <h2>DỊCH VỤ SỬA CHỮA GIÀY</h2>       <p>Nếu giày của bạn bị hư hỏng như rách, đứt dây hoặc đế bị mòn, chúng tôi cung cấp dịch vụ sửa chữa giày chuyên nghiệp để giúp giày của bạn trở lại trạng thái hoàn hảo, như mới, với chi phí hợp lý và thời gian nhanh chóng.</p>       <img src='/images/Caring/san-pham-phuc-hoi-giay-toan-dien.jpg' alt='img6' />", ServiceID = 3, Link = "/MovingSupport/Index" },
                    new ServiceType { Name = "Chăm sóc khách hàng di động", Description = "  <p>Với mạng lưới cửa hàng và dịch vụ trải dài khắp các thành phố lớn và tỉnh thành, Nike là một trong những thương hiệu thể thao có độ phủ rộng nhất, mang đến dịch vụ chăm sóc giày chuyên nghiệp đến tay khách hàng. Để đáp ứng nhu cầu khách hàng, dịch vụ Nike Mobile Service mang đến sự tiện lợi với các dịch vụ bảo dưỡng và sửa chữa giày ngay tại nhà hoặc nơi làm việc của bạn, giúp bạn tiết kiệm thời gian và luôn giữ đôi giày của mình trong tình trạng tốt nhất.</p>       <img src='/images/Caring/nike-hanh-trinh-chuyen-doi-so.jpg' alt='img1' />       <p>Chúng tôi hiểu rằng, giữa cuộc sống bận rộn, thời gian của bạn vô cùng quý giá. Vì vậy, dịch vụ Nike Mobile Service mang đến các dịch vụ bảo dưỡng giày chính hãng với chi phí không đổi, giống như khi bạn đến cửa hàng. Các dịch vụ bao gồm:</p>       <p>- Kiểm tra tình trạng giày bằng công nghệ chuyên dụng.</p>       <p>- Bảo dưỡng định kỳ và sửa chữa giày bị hư hỏng nhỏ.</p>       <p>- Tư vấn về cách chăm sóc và bảo quản giày đúng cách.</p>       <p>- Chia sẻ kinh nghiệm xử lý giày bị hư hỏng trong quá trình sử dụng.</p>      <img src='/images/Caring/Nike-Run-Club.jpg' alt='img3' />       <p>Kể từ ngày đầu tiên, dịch vụ Nike Mobile Service đã thu hút nhiều khách hàng từ các khu vực xa các cửa hàng Nike chính hãng như Hà Nội, Hồ Chí Minh, Đà Nẵng, Nha Trang, và các tỉnh khác. Trong tương lai, Nike Mobile Service sẽ tiếp tục mở rộng để phục vụ nhiều khách hàng hơn trên khắp cả nước.</p>       <p>Để biết thêm thông tin chi tiết về dịch vụ cũng như lịch trình, khách hàng có thể liên hệ tổng đài 0934 1900 61.</p>", ServiceID = 3, Link = "/SupportHotline/Index" },
                    new ServiceType { Name = "Xem chi tiết", Description = " <p>Trong xã hội hiện đại, thời gian là vô cùng quý giá. Hiểu được điều này, chúng tôi luôn khuyến khích khách hàng đặt lịch hẹn dịch vụ để tiết kiệm thời gian và đảm bảo rằng giày của bạn sẽ được chăm sóc một cách tốt nhất. Dịch vụ của chúng tôi cam kết sẽ mang lại sự hài lòng tuyệt đối, giúp đôi giày của bạn luôn trong tình trạng tốt nhất.</p>       <h2>Đặt hẹn trực tuyến qua ứng dụng Nike Service:</h2>       <img src='/images/Caring/phan-mem-dat-lich-hen-3-min.jpg' alt='img1' />       <p>Với ứng dụng Nike Service, chỉ cần mang theo điện thoại di động, quý khách có thể dễ dàng khám phá thông tin về giày của mình và đặt hẹn dịch vụ mọi lúc mọi nơi. Quý khách chỉ cần chọn cửa hàng yêu thích, lựa chọn dịch vụ và ghi chú yêu cầu của mình, rồi chọn thời gian thuận tiện nhất. Sau khi hẹn được xác nhận, quý khách sẽ nhận thông báo tự động để không bỏ lỡ lịch hẹn.</p>       <p>Hãy tải ứng dụng Nike Service ngay hôm nay và khám phá nhiều tiện ích giúp chăm sóc giày của bạn dễ dàng hơn.</p>       <p>App Store (iOS): https://apps.apple.com/vn/app/nike-service/id1234567890</p>       <p>Google Play (Android): https://bit.ly/3NikeApp</p>      <h2>Đặt hẹn trực tiếp:</h2>       <p>Quý khách có thể đặt hẹn qua số Hotline Dịch vụ tại cửa hàng Nike gần nhất hoặc gọi tổng đài CSKH 1800 1234. Nhân viên chăm sóc khách hàng sẽ giúp quý khách xác nhận lịch hẹn dịch vụ và nhắc lại lịch hẹn trước khi đến cửa hàng.</p>       <img src='/images/Caring/thiế1676712879147.jpg' alt='img2' />", ServiceID = 4, Link = "/MeetingDetail/Index" }
                );
                _context.SaveChanges(); // Save categories to ensure IDs are generated
            }

            if (!_context.ProductColor.Any())
            {
                _context.ProductColor.AddRange(
                    new ProductColor { Color = "Red" },
                    new ProductColor { Color = "Blue" },
                    new ProductColor { Color = "Green" },
                    new ProductColor { Color = "Black" },
                    new ProductColor { Color = "White" }
                );
                _context.SaveChanges();
            }

            // Retrieve category IDs after saving them
            var Red = _context.ProductColor.FirstOrDefault(c => c.Color == "Red");
            var Blue = _context.ProductColor.FirstOrDefault(c => c.Color == "Blue");
            var Green = _context.ProductColor.FirstOrDefault(c => c.Color == "Green");
            var Black = _context.ProductColor.FirstOrDefault(c => c.Color == "Black");
            var White = _context.ProductColor.FirstOrDefault(c => c.Color == "White");

            // Add Product Genders if they don't exist
            if (!_context.ProductGender.Any())
            {
                _context.ProductGender.AddRange(
                    new ProductGender { GenderName = "Men" },
                    new ProductGender { GenderName = "Women" },
                    new ProductGender { GenderName = "Unisex" }
                );
                _context.SaveChanges();
            }

            // Retrieve category IDs after saving them
            var Men = _context.ProductGender.FirstOrDefault(c => c.GenderName == "Men");
            var Women = _context.ProductGender.FirstOrDefault(c => c.GenderName == "Women");
            var Unisex = _context.ProductGender.FirstOrDefault(c => c.GenderName == "Unisex");

            // Add Product Sizes if they don't exist
            if (!_context.ProductSize.Any())
            {
                _context.ProductSize.AddRange(
                    new ProductSize { Size = "40" },
                    new ProductSize { Size = "41" },
                    new ProductSize { Size = "42" },
                    new ProductSize { Size = "43" },
                    new ProductSize { Size = "44" }
                );
                _context.SaveChanges();
            }

            // Retrieve category IDs after saving them
            var size40 = _context.ProductSize.FirstOrDefault(c => c.Size == "40");
            var size41 = _context.ProductSize.FirstOrDefault(c => c.Size == "41");
            var size42 = _context.ProductSize.FirstOrDefault(c => c.Size == "42");
            var size43 = _context.ProductSize.FirstOrDefault(c => c.Size == "43");
            var size44 = _context.ProductSize.FirstOrDefault(c => c.Size == "44");

            // Add Product Types if they don't exist
            if (!_context.ProductType.Any())
            {
                _context.ProductType.AddRange(
                    new ProductType { Type = "High" },
                    new ProductType { Type = "Low" },
                    new ProductType { Type = "Middle" }
                );
                _context.SaveChanges();
            }

            // Retrieve category IDs after saving them
            var High = _context.ProductType.FirstOrDefault(c => c.Type == "High");
            var Low = _context.ProductType.FirstOrDefault(c => c.Type == "Low");
            var Middle = _context.ProductType.FirstOrDefault(c => c.Type == "Middle");

            // Add Products if they don't exist
            if (!_context.Product.Any())
            {
                var airForce1 = new Product
                {
                    Name = "Nike Air Force 1",
                    Slug = "airforce1",
                    Description = "💖 SHOP CAM KẾT: <br />\r\n\t\t\t\t\t✔ Cam kết chất lượng, hoàn tiền 100% khi không hài lòng về sản phẩm. <br />\r\n\t\t\t\t\t✔ Bao check/test đổi trả 30 ngày (sản phẩm còn nguyên tem, chưa qua sử dụng). <br />\r\n\t\t\t\t\t✔ Đa dạng mẫu mã, giá SALE RẺ nhất thị trường. <br />\r\n\t\t\t\t\t✔ Giao hàng nhanh đúng tiến độ không phải để quý khách chờ đợi lâu. <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t💖Thông Tin sản phẩm: <br />\r\n\t\t\t\t\t- MÃ SẢN PHẨM: 314192-117 <br />\r\n\t\t\t\t\t- Chất Liệu Upper: Da PU Cao Cấp <br />\r\n\t\t\t\t\t- Chất Liệu Đế: Cao Su <br />\r\n\t\t\t\t\t- Kiểu Dáng : Cổ Thấp ( Low) <br />\r\n\t\t\t\t\t- Tình Trạng: Mới 100% Fullbox <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t3 LÝ DO KHIẾN AF1 ALL WHITE LUÔN HOT TỘT ĐỘ: <br />\r\n\t\t\t\t\t👉 Tính ứng dụng cao. <br />\r\n\t\t\t\t\tDù bạn là nam hay nữ ở bất kì độ tuổi nao thì AF1 Low White vẫn sẽ phát huy được tác dụng của mình. Bạn có thể đơn giản mặc mọi bộ đồ bạn yêu thích với AF1. <br />\r\n\t\t\t\t\t👉 Công nghệ air đệm khí. <br />\r\n\t\t\t\t\tMột lý do khiến các bạn trẻ lựa chọn mẫu Sneaker này đơn giản vì nó đi siêu êm chân. Nhờ công nghệ Air đặc biệt, bạn có thể hoạt động cả ngày vẫn có thể cảm nhận được sự thông thoáng và thoải mái. <br />\r\n\t\t\t\t\t👉 Thiết kế - phối màu. <br />\r\n\t\t\t\t\tChúng ta ai cũng phải công nhận thiết kế của AF1 là quá đẹp đi, nó vừa đơn giản tinh tế mà lại trông rất cá tính. Chưa kể một đôi giày trắng luôn là món đồ cần thiết trong tủ giày của mội người. <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t💖CÁC TRƯỜNG HỢP ÁP DỤNG BẢO HÀNH TẠI OXO FASHION\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t✔ Shop cho đổi lại hàng khác trong vòng 7 ngày kể từ ngày mua. <br />\r\n\t\t\t\t\t✔ Sản phẩm giao sai về số lượng, sai thông tin và mẫu mã so với đơn đặt hàng khách đặt. <br />\r\n\t\t\t\t\t✔  Sản phẩm bị hỏng do lỗi nhà sản xuất (lỗi về thiết kế, chất liệu da hoặc vải bị rách hoặc bị bong tróc, khác biệt so với hình ảnh đăng tải trên website hoặc lỗi trong quá trình vận chuyển (bị biến dạng, trầy xước, vấy bẩn, nứt vỡ ...)\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t✔  Khách hàng mua không vừa kích cỡ (size) sẽ được đổi size. <br />\r\n\t\t\t\t\t✔  Sản phẩm mới 100% và chưa qua sử dụng. <br />",
                    Price = 200,
                    CategoryID = airForceCategory.CategoryID, 
                    ProductColorID = White.ProductColorID, 
                    ProductSizeID = size41.ProductSizeID, 
                    ProductTypeID = Low.ProductTypeID, 
                    GenderID = Men.GenderID, 
                    WarehouseID = 1,
                    PromotionID = 1,
                    IsHot = true,
                    IsFavorite = true,
                    ImageUrl = "AIR FORCE 1 07.jpg",
                };

                var airJordan1 = new Product
                {
                    Name = "Nike Air Jordan 1",
                    Slug = "airjordan1",
                    Description = "💖 SHOP CAM KẾT22222: <br />\r\n\t\t\t\t\t✔ Cam kết chất lượng, hoàn tiền 100% khi không hài lòng về sản phẩm. <br />\r\n\t\t\t\t\t✔ Bao check/test đổi trả 30 ngày (sản phẩm còn nguyên tem, chưa qua sử dụng). <br />\r\n\t\t\t\t\t✔ Đa dạng mẫu mã, giá SALE RẺ nhất thị trường. <br />\r\n\t\t\t\t\t✔ Giao hàng nhanh đúng tiến độ không phải để quý khách chờ đợi lâu. <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t💖Thông Tin sản phẩm: <br />\r\n\t\t\t\t\t- MÃ SẢN PHẨM: 314192-117 <br />\r\n\t\t\t\t\t- Chất Liệu Upper: Da PU Cao Cấp <br />\r\n\t\t\t\t\t- Chất Liệu Đế: Cao Su <br />\r\n\t\t\t\t\t- Kiểu Dáng : Cổ Thấp ( Low) <br />\r\n\t\t\t\t\t- Tình Trạng: Mới 100% Fullbox <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t3 LÝ DO KHIẾN AF1 ALL WHITE LUÔN HOT TỘT ĐỘ: <br />\r\n\t\t\t\t\t👉 Tính ứng dụng cao. <br />\r\n\t\t\t\t\tDù bạn là nam hay nữ ở bất kì độ tuổi nao thì AF1 Low White vẫn sẽ phát huy được tác dụng của mình. Bạn có thể đơn giản mặc mọi bộ đồ bạn yêu thích với AF1. <br />\r\n\t\t\t\t\t👉 Công nghệ air đệm khí. <br />\r\n\t\t\t\t\tMột lý do khiến các bạn trẻ lựa chọn mẫu Sneaker này đơn giản vì nó đi siêu êm chân. Nhờ công nghệ Air đặc biệt, bạn có thể hoạt động cả ngày vẫn có thể cảm nhận được sự thông thoáng và thoải mái. <br />\r\n\t\t\t\t\t👉 Thiết kế - phối màu. <br />\r\n\t\t\t\t\tChúng ta ai cũng phải công nhận thiết kế của AF1 là quá đẹp đi, nó vừa đơn giản tinh tế mà lại trông rất cá tính. Chưa kể một đôi giày trắng luôn là món đồ cần thiết trong tủ giày của mội người. <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t💖CÁC TRƯỜNG HỢP ÁP DỤNG BẢO HÀNH TẠI OXO FASHION\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t✔ Shop cho đổi lại hàng khác trong vòng 7 ngày kể từ ngày mua. <br />\r\n\t\t\t\t\t✔ Sản phẩm giao sai về số lượng, sai thông tin và mẫu mã so với đơn đặt hàng khách đặt. <br />\r\n\t\t\t\t\t✔  Sản phẩm bị hỏng do lỗi nhà sản xuất (lỗi về thiết kế, chất liệu da hoặc vải bị rách hoặc bị bong tróc, khác biệt so với hình ảnh đăng tải trên website hoặc lỗi trong quá trình vận chuyển (bị biến dạng, trầy xước, vấy bẩn, nứt vỡ ...)\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t✔  Khách hàng mua không vừa kích cỡ (size) sẽ được đổi size. <br />\r\n\t\t\t\t\t✔  Sản phẩm mới 100% và chưa qua sử dụng. <br />",
                    Price = 150,
                    CategoryID = jordanCategory.CategoryID, 
                    ProductColorID = Black.ProductColorID, 
                    ProductSizeID = size42.ProductSizeID, 
                    ProductTypeID = Middle.ProductTypeID, 
                    GenderID = Women.GenderID, 
                    WarehouseID = 2,
                    PromotionID = 1,
                    IsHot = true,
                    IsFavorite = true,
                    ImageUrl = "W AIR MAX DN ISA.jpg",
                };

                var dunk = new Product
                {
                    Name = "Nike Dunk",
                    Slug = "dunk",
                    Description = "💖 SHOP CAM KẾT3333: <br />\r\n\t\t\t\t\t✔ Cam kết chất lượng, hoàn tiền 100% khi không hài lòng về sản phẩm. <br />\r\n\t\t\t\t\t✔ Bao check/test đổi trả 30 ngày (sản phẩm còn nguyên tem, chưa qua sử dụng). <br />\r\n\t\t\t\t\t✔ Đa dạng mẫu mã, giá SALE RẺ nhất thị trường. <br />\r\n\t\t\t\t\t✔ Giao hàng nhanh đúng tiến độ không phải để quý khách chờ đợi lâu. <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t💖Thông Tin sản phẩm: <br />\r\n\t\t\t\t\t- MÃ SẢN PHẨM: 314192-117 <br />\r\n\t\t\t\t\t- Chất Liệu Upper: Da PU Cao Cấp <br />\r\n\t\t\t\t\t- Chất Liệu Đế: Cao Su <br />\r\n\t\t\t\t\t- Kiểu Dáng : Cổ Thấp ( Low) <br />\r\n\t\t\t\t\t- Tình Trạng: Mới 100% Fullbox <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t3 LÝ DO KHIẾN AF1 ALL WHITE LUÔN HOT TỘT ĐỘ: <br />\r\n\t\t\t\t\t👉 Tính ứng dụng cao. <br />\r\n\t\t\t\t\tDù bạn là nam hay nữ ở bất kì độ tuổi nao thì AF1 Low White vẫn sẽ phát huy được tác dụng của mình. Bạn có thể đơn giản mặc mọi bộ đồ bạn yêu thích với AF1. <br />\r\n\t\t\t\t\t👉 Công nghệ air đệm khí. <br />\r\n\t\t\t\t\tMột lý do khiến các bạn trẻ lựa chọn mẫu Sneaker này đơn giản vì nó đi siêu êm chân. Nhờ công nghệ Air đặc biệt, bạn có thể hoạt động cả ngày vẫn có thể cảm nhận được sự thông thoáng và thoải mái. <br />\r\n\t\t\t\t\t👉 Thiết kế - phối màu. <br />\r\n\t\t\t\t\tChúng ta ai cũng phải công nhận thiết kế của AF1 là quá đẹp đi, nó vừa đơn giản tinh tế mà lại trông rất cá tính. Chưa kể một đôi giày trắng luôn là món đồ cần thiết trong tủ giày của mội người. <br />\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t💖CÁC TRƯỜNG HỢP ÁP DỤNG BẢO HÀNH TẠI OXO FASHION\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t✔ Shop cho đổi lại hàng khác trong vòng 7 ngày kể từ ngày mua. <br />\r\n\t\t\t\t\t✔ Sản phẩm giao sai về số lượng, sai thông tin và mẫu mã so với đơn đặt hàng khách đặt. <br />\r\n\t\t\t\t\t✔  Sản phẩm bị hỏng do lỗi nhà sản xuất (lỗi về thiết kế, chất liệu da hoặc vải bị rách hoặc bị bong tróc, khác biệt so với hình ảnh đăng tải trên website hoặc lỗi trong quá trình vận chuyển (bị biến dạng, trầy xước, vấy bẩn, nứt vỡ ...)\r\n\t\t\t\t\t<br />\r\n\t\t\t\t\t✔  Khách hàng mua không vừa kích cỡ (size) sẽ được đổi size. <br />\r\n\t\t\t\t\t✔  Sản phẩm mới 100% và chưa qua sử dụng. <br />",
                    Price = 180,
                    CategoryID = dunkCategory.CategoryID, 
                    ProductColorID = Black.ProductColorID, 
                    ProductSizeID = size42.ProductSizeID, 
                    ProductTypeID = Middle.ProductTypeID, 
                    GenderID = Women.GenderID, 
                    WarehouseID = 2,
                    PromotionID = 2,
                    IsHot = false,
                    IsFavorite = true,
                    ImageUrl = "W NIKE DUNK LOW NEXT NATURE.jpg",
                };
                _context.Product.AddRange(airForce1, airJordan1, dunk);
                _context.SaveChanges();
            }

            // Add Product Categories (Collections) if they don't exist
            if (!_context.Importing.Any()) // Kiểm tra xem có dữ liệu chưa
            {
                _context.Importing.AddRange(
                   new Importing
                   {
                       ProviderID = 1,
                       WarehouseID = 1,
                       ImportDate = DateTime.Now,
                       TotalAmount = 30000.00m,
                       Status = "Completed"
                   },
                    new Importing
                    {
                        ProviderID = 2,
                        WarehouseID = 2,
                        ImportDate = DateTime.Now,
                        TotalAmount = 20000.00m,
                        Status = "Pending"
                    }
                );
                _context.SaveChanges(); // Lưu dữ liệu vào DB
            }
        }
    }
}
