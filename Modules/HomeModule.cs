using System.Collections.Generic;
using System;
using Nancy;
using CuisineFinder.Objects;

namespace CuisineFinder
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Cuisine> AllCuisines = Cuisine.GetAll();
        return View["index.cshtml", AllCuisines];
      };
      Get["/restaurants"] = _ => {
        List<Restaurant> AllRestaurants = Restaurant.GetAll();
        return View["restaurants.cshtml", AllRestaurants];
      };
      Get["/cuisines"] = _ => {
        List<Cuisine> AllCuisines = Cuisine.GetAll();
        return View["cuisines.cshtml", AllCuisines];
      };
      Get["/cuisines/new"] = _ => {
        return View["cuisines_form.cshtml"];
      };
      Post["/cuisines/new"] = _ => {
        Cuisine newCuisine = new Cuisine(0, Request.Form["cuisine-name"]);
        newCuisine.Save();
        return View["success.cshtml"];
      };
      Get["/restaurants/new"] = _ => {
        List<Cuisine> AllCuisines = Cuisine.GetAll();
        return View["restaurant_form.cshtml", AllCuisines];
      };
      Post["/restaurants/new"] = _ => {
        Restaurant newRestaurant = new Restaurant(Request.Form["cuisine-id"], Request.Form["restaurant-name"], Request.Form["restaurant-image"]);
        newRestaurant.Save();
        return View["success.cshtml"];
      };
      Post["/restaurants/delete"] = _ => {
        Restaurant.DeleteAll();
        return View["cleared.cshtml"];
      };
      Post["/cuisines/delete"] = _ => {
        Cuisine.DeleteAll();
        return View["cleared.cshtml"];
      };
      Get["/cuisines/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedCuisine = Cuisine.Find(parameters.id);
        var CuisineRestaurants = SelectedCuisine.GetRestaurants();
        model.Add("cuisine", SelectedCuisine);
        model.Add("restaurants", CuisineRestaurants);
        return View["cuisine.cshtml", model];
      };
      Get["/cuisine/edit/{id}"] = parameters => {
        Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
        return View["cuisine_edit.cshtml", SelectedCuisine];
      };
      Patch["/cuisine/edit/{id}"] = parameters => {
        Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
        SelectedCuisine.Update(Request.Form["cuisine-name"]);
        return View["success.cshtml"];
      };
      Get["/cuisine/delete/{id}"] = parameters => {
        Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
        return View["cuisine_delete.cshtml", SelectedCuisine];
      };
      Delete["/cuisine/delete/{id}"] = parameters => {
        Cuisine SelectedCuisine = Cuisine.Find(parameters.id);
        SelectedCuisine.Delete();
        return View["success.cshtml"];
      };
      Get["/restaurant/review/{id}"] = parameters => {
        var SelectedRestaurant = Restaurant.Find(parameters.id);
        return View["review_form.cshtml", SelectedRestaurant];
      };
      Post["/new-review/{id}"] = parameters => {
        var SelectedRestaurant = Restaurant.Find(parameters.id);
        Review newReview = new Review(Request.Form["stars"], Request.Form["restaurant-review"], SelectedRestaurant.GetId());
        newReview.Save();
        return View["review_success.cshtml", newReview.GetComment()];
      };
      Get["/restaurants/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var SelectedRestaurant = Restaurant.Find(parameters.id);
        var RestaurantReviews = SelectedRestaurant.GetReviews();
        model.Add("restaurant", SelectedRestaurant);
        model.Add("reviews", RestaurantReviews);
        return View["restaurant.cshtml", model];
      };
    }
  }
}
