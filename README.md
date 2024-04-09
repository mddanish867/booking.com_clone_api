Search functionality


1- Created SearchContext in frontend to save values
2- npm i react-datepicker
3- Created SearchBar component inside Component folder
4- Created Search component inside pages folder and call this inside App.tsx


Now Create seach API

5- Created ISearchRepository 
6- Created SearchRepository
7- Created SearchController
8- Now Register ISearchRepository and SearchRepository in Program.cs

Now create fetch api inside frontend

9- created new Api with the name of searchHotels inside hotelAPI
10- use useSearchHotelsQuery it inside Search component
11- now create search card named as SearchResultCard.tsx and implement the functionality

Now implement filters 

12- Created new component named as StarRatingFilter and implement the functionality
13- Created new component named as HotelTypeFilter and implement the functionality
14- Created new component named as FacilitiesFilter and implement the functionality
15- Created new component named as Pagination and implement the functionality
16- Created new component named as PriceFilter and implement the functionality
17- Now integrate these component in Search page
18- Now create Sorting dropdown in Search Page

19- Now created new component named as Details add routes inside App.tsx and implement functionality by calling this useGetHotelByIdQuery API
20- Add new component GuestInForm for Booking the and implement the logic
21- Save user search value in session storage so for ths modify following component and pages SearchContext
22- Now created new component names as Booking for booking confirmation once click on reserve button from details page and add routes for booking in App.tsx
23- and now create new form named as BookingForm inside Form Folder 

24- Create new table named as Address by using this query 
CREATE TABLE Addresses (
    AddressId UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Email NVARCHAR(255),
    Mobile NVARCHAR(20),
    IsDefault BIT
);

25- and create new model inside API named as UserAddress
26- created new Dto inside Model named as UserAddressDto
27- add new method inside userRepository named as AddUserAddressAsync
28- add new method inside IUserRepository named as AddUserAddressAsync
29- add new endpoint to add address named as AddUserAddress
30- add new modal popup in Booking.tsx to add new address
31- add new api end point int authApi to addUserAddress
32- integrate the adduserAddress API into add new address modal popup
33- add add new method in Controller named as GetLatestUserAddress
34 add new method inside userRepository named as GetLatestUserAddressAsync
35- add new method inside IUserRepository named as GetLatestUserAddressAsync
36- Integrate this method inside BookingForm.tsx
