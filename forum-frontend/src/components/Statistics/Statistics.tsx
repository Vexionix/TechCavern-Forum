import { useEffect, useState } from "react";
import api from "../../utils/api";
import { Link } from "react-router-dom";
import DynamicGiIcon from "../../utils/iconLoader";

import "./Categories.css";
import timeAgo from "../../utils/timeAgo";

interface Subcategory {
  id: number;
  name: string;
  description: string;
  giIcon: string;
  numberOfPosts: number;
  numberOfComments: number;
  postWithMostRecentActivity: PostData | undefined;
}

interface Category {
  id: number;
  name: string;
  giIcon: string;
  subcategories: Subcategory[];
}

interface PostData {
  id: number;
  title: string;
  username: string;
  userId: number;
  latestActivityPostedAt: string;
}

const Categories = () => {
  const [categories, setCategories] = useState<Category[]>([]);

  useEffect(() => {
    const controller = new AbortController();

    const getCategories = async () => {
      try {
        const response = await api.get("/categories/with-subcategories", {
          signal: controller.signal,
        });
        setCategories(response.data);
      } catch (err: any) {
        if (err.code !== "ERR_CANCELED" && err.name !== "CanceledError") {
          console.error("Error fetching categories:", err);
        }
      }
    };

    getCategories();

    return () => {
      controller.abort();
    };
  }, []);

  return (
    <div className="category-wrapper">
      {categories.map((category) => (
        <div className="category-wrapper" key={"category-" + category.id}>
          <div className="category-title">
            <h3>
              <DynamicGiIcon name={category.giIcon} />
              &nbsp; {category.name}
            </h3>
          </div>
          <ul>
            {category.subcategories.map((subcategory, subcategoryIndex) => {
              const isLast =
                subcategoryIndex === category.subcategories.length - 1;
              return (
                <div key={"subcategory-" + subcategory.id}>
                  <div className="subcategory-container">
                    <li>
                      <div className="icon-with-main">
                        <p>
                          <DynamicGiIcon name={subcategory.giIcon} />
                        </p>
                        <div className="subcategory-info">
                          <Link
                            to={"/subcategory/" + subcategory.id}
                            className="subcategory-title"
                          >
                            {subcategory.name}
                          </Link>
                          <span className="subcategory-description">
                            {subcategory.description}
                          </span>
                        </div>
                      </div>
                      <div className="activity-and-latest">
                        <div className="subcategory-phone-activity-separator"></div>
                        <div className="subcategory-activity">
                          <div className="header-subcategory-posts">
                            <p>
                              Posts:{" "}
                              <span className="subcategory-posts-value">
                                {subcategory.numberOfPosts}
                              </span>
                            </p>
                          </div>
                          <div className="header-subcategory-comments">
                            <p>
                              Comments:{" "}
                              <span className="subcategory-comments-value">
                                {subcategory.numberOfComments}
                              </span>{" "}
                            </p>
                          </div>
                        </div>
                        <p className="subcategory-info-latest-separator"></p>
                        {subcategory.postWithMostRecentActivity ? (
                          <div className="subcategory-latests">
                            <Link
                              to={
                                "/post/" +
                                subcategory.postWithMostRecentActivity.id
                              }
                            >
                              {subcategory.postWithMostRecentActivity.title}
                            </Link>
                            <div className="header-subcategory-posts">
                              <p>
                                <Link
                                  to={
                                    "/user/profile/" +
                                    subcategory.postWithMostRecentActivity
                                      .userId
                                  }
                                >
                                  {
                                    subcategory.postWithMostRecentActivity
                                      .username
                                  }
                                </Link>
                                <span>
                                  {" "}
                                  ●{" "}
                                  {timeAgo(
                                    subcategory.postWithMostRecentActivity
                                      .latestActivityPostedAt
                                  )}{" "}
                                </span>
                              </p>
                            </div>
                          </div>
                        ) : (
                          <div className="no-posts">
                            No post was added here yet!
                          </div>
                        )}
                      </div>
                    </li>
                  </div>
                  {!isLast && (
                    <div className="separator-div">
                      <p className="subcategory-separator"></p>
                    </div>
                  )}
                </div>
              );
            })}
          </ul>
        </div>
      ))}
    </div>
  );
};

export default Categories;
