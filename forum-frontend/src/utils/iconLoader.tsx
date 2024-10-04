import * as Icons from "react-icons/gi";
import { IconType } from "react-icons";

interface Props {
  name: string;
}

const DynamicGiIcon = ({ name }: Props) => {
  const IconComponent = (Icons as Record<string, IconType>)[name];

  if (!IconComponent) {
    return <Icons.GiDiplodocus />;
  }

  return <IconComponent />;
};

export default DynamicGiIcon;
