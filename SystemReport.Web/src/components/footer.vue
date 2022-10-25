<template>
  <footer class="page-footer hd-screen" >
    <div class=" bg-footer ">
      <div class="container-fluid">
        <div class="row">
          <div class="col-12">
            <div class="layout wrap row">
              <div class="flex text-center">
                <div class="organization text-uppercase">Sở Khoa học và Công nghệ Đồng Tháp</div>
                <div class="general-info"><strong>Địa chỉ:</strong> Số 5-7, đường Hai Bà Trưng, phường 2, thành phố Cao Lãnh, tỉnh Đồng Tháp
                </div>
                <div class="infos"><strong>Điện thoại:</strong> 0277.3 851<span> | </span><strong>E-mail:</strong>
                  skhcn@dongthap.gov.vn
                </div>
                <div class="copyright">© {{ new Date().getFullYear() }}, Sở Khoa học và Công nghệ Đồng Tháp</div>
              </div>
            </div>

          </div>
        </div>
      </div>
    </div>

  </footer>
  <!-- end footer -->
</template>

<style>
.page-footer{
  background-color: #111961 !important;
}
.bg-footer {

  background: linear-gradient(0deg, rgba(149, 167, 225, 0.1), rgba(149, 167, 225, 0.1)) !important;
  border-bottom: 5px solid #1542b2;
  margin-bottom: 0;
  padding: 40px;
  background-size: cover;
  color: white;
  width: 100%;
  font-size: 13px;
  display: block;
}

.page-footer .organization {
  width: 100%;
  font-weight: 800;
  margin-bottom: 10px;
  font-size: 18px;
}
</style>