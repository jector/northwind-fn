/* View users cart controll */
if (document.getElementById('User').dataset['customer'].toLowerCase() == "true") {

  const tbody = document.querySelector('tbody');


  

  document.addEventListener("DOMContentLoaded", function () {
    fetchCartProducts();
  });

  async function fetchCartProducts() {
    const id = document.getElementById('listId').firstElementChild.dataset['id'];
    const { data: fetchCartProducts } = await axios.get(`../../api/cart/customer/${id}/`);

    let cart_rows = "";
    fetchCartProducts.map(cart => {
      cart_rows +=
        `<tr id="${cart.productId}" data-id="${cart.cartItemId}">
          <td>${cart.product.productName}</td>
          <td>$<span class="cartUnitPrice" data-id="${cart.product.unitPrice}" data-discount="0.00">${cart.product.unitPrice.toFixed(2)}</span></td>
          <td><input type="number" min="1" value="${cart.quantity}" class="form-control cartQuantityInput" /></td>
          <td>$<span class="result">${(cart.product.unitPrice * cart.quantity).toFixed(2)}</span></td>
          <td class="text-center"><input class="btn btn-danger remove" type="submit" value="Delete"></td>
        </tr>`;
    });
    document.getElementById('cart_rows').innerHTML = cart_rows;

    /* Input update quantity and update total */
    document.getElementById("cartTable").addEventListener("change", function (e) {
      const row = e.target.parentNode.parentNode;
      const val1 = row.querySelector("#cartTable .cartUnitPrice").textContent;
      const val2 = row.querySelector("#cartTable .cartQuantityInput").value;
      row.querySelector("#cartTable .result").innerHTML = parseFloat(val1 * val2).toFixed(2);
      sumTotal();
    });
    /* Sum total */
    const sumTotal = () => {
      const table = document.getElementById("cart_rows");
      const rows = table.getElementsByTagName("tr");
      let sum = 0;
      for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        const cellValue = parseFloat(row.querySelector(".result").textContent);
        sum += cellValue;
      }
      document.getElementById('cartTotal').textContent = parseFloat(sum + 14).toFixed(2);
    }
    sumTotal();

    /* Delete cart item */
    document.getElementById("cartTable").addEventListener("click", function (e) {
      const row = e.target.parentNode.parentNode;
      if (e.target.classList.contains('remove')) {
        deleteCartItem(row.dataset['id']);
        row.remove();
        sumTotal();
      }
    });

    async function deleteCartItem(id) {
      const data = await axios.delete(`../../api/cart/delete/${id}`)
      console.log(data);
    }

    /* Fetch discounted products*/
    const table = document.getElementById("cart_rows");
    const rows = table.getElementsByTagName("tr");
    for (let i = 0; i < rows.length; i++) {
      const row = rows[i];
      const rowId = row.id;
      //console.log(rowId);
      const { data: fetchprodductDiscounts } = await axios.get(`../../api/cart/discounted/${rowId}`)
      let discount_div = "";
      //console.log(fetchprodductDiscounts);
      fetchprodductDiscounts.map(discount => {
        discount_div +=
          `<div class="discount col" data-id="${discount.productId}">
              <h5>${discount.title}</h5>
              <h6 class="discountPercent text-body-secondary" data-id="${discount.discountPercent}">${discount.discountPercent * 100}%</h6>
              <p>${discount.description}</p>
              <p><input class="btn btn-outline-success addDiscount" type="button" value="Add Discount"></p>
          </div>`;
      });
      //console.log(discount_div);
      document.getElementById('cartDiscount').innerHTML += discount_div;

      let discount_list = "";
      fetchprodductDiscounts.map(discount => {
        discount_list +=
          `<li class="list-group-item listControl" data-id="${discount.productId}">
              <span class="discountPercent" data-id="${discount.discountPercent}">${discount.discountPercent * 100}%</span> ${discount.product.productName}
          </li>`;
      });
      document.getElementById('discountDetails').innerHTML += discount_list;
    }
    /* Add discount */
    let isToggled = false;
    document.getElementById("cartDiscount").addEventListener("click", function (e) {
      isToggled = !isToggled;

      const table = document.getElementById("cart_rows");
      const rows = table.getElementsByTagName("tr");
      const divContainer = e.target.parentNode.parentNode;
      const disContainerId = divContainer.dataset['id'];

      for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        const rowId = row.id;
        const val1 = row.querySelector("#cartTable .cartUnitPrice").dataset['id'];
        const val2 = divContainer.querySelector("#cartDiscount .discountPercent").dataset['id'];
        const val3 = row.querySelector("#cartTable .cartQuantityInput").value;

        //console.log(disContainerId);
        if (isToggled) {
          if (disContainerId == rowId) {
            row.classList.add("newColor");
            row.querySelector("#cartTable .cartUnitPrice").dataset['discount'] = val2;
            row.querySelector("#cartTable .cartUnitPrice").innerHTML = parseFloat(val1 - (val1 * val2)).toFixed(2) + " - " + (val2 * 100) + "%";
            row.querySelector("#cartTable .result").innerHTML = parseFloat(val1 - (val1 * val2)).toFixed(2);
          }
        } else {
          if (disContainerId == rowId) {
            row.classList.remove("newColor");
            row.querySelector("#cartTable .cartUnitPrice").dataset['discount'] = "0.00";
            row.querySelector("#cartTable .cartUnitPrice").innerHTML = parseFloat(val1).toFixed(2);
            row.querySelector("#cartTable .result").innerHTML = parseFloat(val1 * val3).toFixed(2);
          }
        }
      }
      sumTotal();
    });

    // Get today's date
    const today = new Date();
    // Add days
    const threeDaysLater = new Date(today);
    threeDaysLater.setDate(today.getDate() + 3);
    const fourteenDaysLater = new Date(today);
    fourteenDaysLater.setDate(today.getDate() + 14);
    // Order dates
    const oderDate = today.toISOString().slice(0, 23);
    //const oderDate = today.toISOString().slice(0, 23).replace('T', ' ');
    const shipedDate = threeDaysLater.toISOString().slice(0, 23);
    const requiredDate = fourteenDaysLater.toISOString().slice(0, 23);

    /* Fetch shipping information */
    const { data: fetchShipDetails } = await axios.get(`../../api/customer/${id}`);
    let shipInformation = "";
    fetchShipDetails.map(ship => {
      shipInformation += `
        <div id="customerDitails" data-number="${ship.customerId}">
        Your order will be shipped to:<br>
          <ul>
            <li class="list-group-item text-body-secondary fs-6"><span id="shipName">${ship.companyName}</span><br>
            <span id="shipAddress">${ship.address}</span>, <span id="shipCity">${ship.city}</span><br>
            <span id="shipRegion" data-id="${ship.region}">${ship.region}</span> zip <span id="shipPostalCode">${ship.postalCode}</span>, <span id="shipCountry">${ship.country}</span>
            </li>
          </ul>
          The freight cost is $<span id="freightCost">14.00</span>
        </div>
        `
    });
    document.getElementById('shipDetails').innerHTML = shipInformation;
    /*const nullRegion = document.querySelector('#shipDetails #shipRegion');
    if (nullRegion.innerHTML == "null") {
      nullRegion.innerHTML = "";
    }*/

    /* Fetch last order */
    const { data: fetchLastOrder } = await axios.get(`../../api/lastOrder/customer/${id}`);
    let lastOrder = "";
    fetchLastOrder.map(order => {
      lastOrder = order;
    });
    document.getElementById('lastOrder').dataset['id'] = lastOrder.orderId;

    /* Add to Order */
    document.getElementById('checkout').addEventListener("click", (e) => {
      // use axios post to add order
      order = {
        "customerId": parseInt(document.getElementById('customerDitails').dataset['number']),
        "employeeId": 3,
        "orderDate": oderDate,
        "requiredDate": shipedDate,
        "shippedDate": requiredDate,
        "shipVia": 3,
        "freight": 14.0000,
        "shipName": document.getElementById('shipName').innerHTML,
        "shipAddress": document.getElementById('shipAddress').innerHTML,
        "shipCity": document.getElementById('shipCity').innerHTML,
        "shipRegion": document.getElementById('shipRegion').innerHTML,
        "shipPostalCode": document.getElementById('shipPostalCode').innerHTML,
        "shipCountry": document.getElementById('shipCountry').innerHTML
      }
      //console.log(order);
      postOrders(order);
    });
    async function postOrders(order) {
      const data = await axios.post('../../api/addtoorder', order)
      //console.log(data);
    }

    /* Add to OrderDetails */
    document.getElementById('checkout').addEventListener("click", (e) => {
      const table = document.getElementById("cart_rows");
      const rows = table.getElementsByTagName("tr");
      for (let i = 0; i < rows.length; i++) {
        const row = rows[i];
        const rowId = row.id;
        const val1 = row.querySelector("#cartTable .cartUnitPrice").dataset['id'];
        const val2 = row.querySelector("#cartTable .cartUnitPrice").dataset['discount'];
        const val3 = row.querySelector("#cartTable .cartQuantityInput").value;
        orderDetails = {
          "orderId": parseInt(document.getElementById('lastOrder').dataset['id']) + 1,
          "productId": rowId,
          "unitPrice": parseInt(val1).toFixed(2),
          "quantity": val3,
          "discount": val2
        }
        //console.log(orderDetails);
        postOrdersDetails(orderDetails);
        const parseIntCustomer = document.getElementById('listId').firstElementChild.dataset['id'];

        deleteallCartItems(parseIntCustomer);
        //console.log(parseIntCustomer);
        document.getElementById('showDiv').innerHTML = "<H3 class='text-center'>Thank you for your order.</H3>";
      }
      async function postOrdersDetails(orderDetails) {
        const data = await axios.post('../../api/addtoorderdetails', orderDetails)
        //console.log(data);
      }

      async function deleteallCartItems(id) {
        const data = await axios.delete(`../../api/deleteallcartitem/CustomerId/${id}`)
        //console.log(data);
      }
    });
  }

setTimeout(() => {
  const tbodyEst = document.getElementById('cart_rows');
  if (tbodyEst.children.length === 0) {
    document.getElementById('showDiv').innerHTML = "<H3 class='text-center'>Your cart is empty.</H3>";
  } 
},"100");
  
  
} else {
  document.getElementById('showDiv').innerHTML = "<H3 class='text-center'>Sorry,<br>You must be signed in as a customer to access the cart.</H3>";
}